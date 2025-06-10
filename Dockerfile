FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .

WORKDIR /app/TrekkingApi.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/logs && chown -R 1000:1000 /app/logs
RUN mkdir -p /app/certs && chown -R 1000:1000 /app/certs

ENTRYPOINT ["dotnet", "TrekkingApi.Api.dll"]

