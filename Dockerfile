# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Set environment variables to handle SSL issues with NuGet
ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

# Copy solution and project files
COPY ["WareHouseManagement.sln", "./"]
COPY ["src/WareHouseManagement.API/WareHouseManagement.API.csproj", "src/WareHouseManagement.API/"]
COPY ["src/WareHouseManagement.Application/WareHouseManagement.Application.csproj", "src/WareHouseManagement.Application/"]
COPY ["src/WareHouseManagement.Domain/WareHouseManagement.Domain.csproj", "src/WareHouseManagement.Domain/"]
COPY ["src/WareHouseManagement.Infrastructure/WareHouseManagement.Infrastructure.csproj", "src/WareHouseManagement.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/WareHouseManagement.API/WareHouseManagement.API.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/src/WareHouseManagement.API"
RUN dotnet build "WareHouseManagement.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "WareHouseManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WareHouseManagement.API.dll"]

