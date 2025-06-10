using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using PeterO.Cbor;
using System.Text.Json;
using TrekkingApi.Application.Converters;

namespace TrekkingApi.Api.Formatter
{
    public class CborInputFormatter : InputFormatter
    {
        private readonly ILogger<CborInputFormatter> _logger;

        public CborInputFormatter(ILogger<CborInputFormatter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/cbor"));
        }

        protected override bool CanReadType(Type type)
        {
            return true; // если хочешь ограничить только DTO — можно проверить typeof(PinUploadDto), и т.д.
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            try
            {
                var httpContext = context.HttpContext;
                using var memoryStream = new MemoryStream();
                await httpContext.Request.Body.CopyToAsync(memoryStream);

                var test = BitConverter.ToString(memoryStream.ToArray()).Replace("-", "");




                var cbor = CBORObject.DecodeFromBytes(memoryStream.ToArray());
                // если это «тэг», разворачиваем его
                while (cbor.TagCount > 0)
                {
                    cbor = cbor.Untag();
                }

                var cborFile = cbor["file"];



                // Переводим CBOR -> JSON -> C#
                var json = cbor.ToJSONString();

                var result = JsonSerializer.Deserialize(json, context.ModelType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new ByteArrayJsonConverter() }
                });

                return await InputFormatterResult.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при разборе CBOR-запроса.");
                return await InputFormatterResult.FailureAsync();
            }
        }
    }
}
