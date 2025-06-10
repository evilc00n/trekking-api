using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Minio;
using Serilog;
using StackExchange.Redis;
using TrekkingApi.Api.Formatter;
using TrekkingApi.Api.Handlers;
using TrekkingApi.Api.Middlewares;
using TrekkingApi.Application.DependencyInjection;
using TrekkingApi.DAL.DependencyInjection;
using TrekkingApi.Domain.Interfaces.Databases;
using TrekkingApi.Domain.Options.MinioOptions;


var builder = WebApplication.CreateBuilder(args);




builder.Services.AddApplications();

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyAuthCookie";
        options.LoginPath = "/api/auth/login-path";
        options.AccessDeniedPath = "/api/auth/access-denied-path";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromDays(30); 
        options.SlidingExpiration = true;
        //options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Domain = "fasberry.su";
        options.Cookie.HttpOnly = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                var sessionService = context.HttpContext
                    .RequestServices.GetRequiredService<ISessionService>();
                var sessionId = context.Principal.FindFirst("SessionId")?.Value;

                if (string.IsNullOrEmpty(sessionId)
                    || !await sessionService.IsSessionValidAsync(sessionId))
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync("MyCookieAuth");
                    return;
                }

                var ttl = await sessionService.GetSessionTtlAsync(sessionId);
                if (ttl.HasValue && ttl.Value < TimeSpan.FromDays(15))
                {
                    await sessionService.RefreshSessionAsync(sessionId);
                    context.ShouldRenew = true;  
                }
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RedisSessionPolicy", policy =>
    {
        policy.Requirements.Add(new RedisSessionRequirement());
    });
});



var redisConnectionString = builder.Configuration["REDIS_CONNECTION_STRING"];
var allowedOrigins = builder.Configuration["ALLOWED_ORIGIN"]?
    .Split(',', StringSplitOptions.RemoveEmptyEntries)
     ?? Array.Empty<string>();


if (string.IsNullOrEmpty(redisConnectionString) || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("Some ENV variables are null.");
}
builder.Services.Configure<MinioSettings>(options =>
{
    options.Endpoint = builder.Configuration["MINIO_ENDPOINT"];
    options.AccessKey = builder.Configuration["MINIO_ACCESS_KEY"];
    options.SecretKey = builder.Configuration["MINIO_SECRET_KEY"];
    options.WithSSL = bool.Parse(builder.Configuration["MINIO_WITH_SSL"] ?? "true");
    options.BucketName = builder.Configuration["MINIO_BUCKET_NAME"];
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

    if (settings == null) throw new InvalidOperationException("Minio settings are null.");

    return new MinioClient()
        .WithEndpoint(settings.Endpoint)
        .WithCredentials(settings.AccessKey, settings.SecretKey)
        .WithSSL(settings.WithSSL)
        .Build(); // возвращает IMinioClient
});

builder.Services.AddControllers(options =>
{
    var provider = builder.Services.BuildServiceProvider();
    var logger = provider.GetRequiredService<ILogger<CborInputFormatter>>();
    options.InputFormatters.Insert(0, new CborInputFormatter(logger));
});


builder.Host.UseSerilog(
    (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MainPolicy", builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, RedisSessionValidator>();
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisConnectionString) 
);


var app = builder.Build();
app.UseCors("MainPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
