# Runtime-only stage - use pre-published files from local build
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy pre-published files (built locally)
COPY publish/ .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WareHouseManagement.API.dll"]

