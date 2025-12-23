# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

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

# Install EF Core tools for migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install dotnet-ef in runtime for migrations
COPY --from=publish /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY --from=publish /app/publish .
COPY --from=build /src/src /src/src

# Create entrypoint script for migrations
RUN echo '#!/bin/bash\n\
echo "Waiting for PostgreSQL to be ready..."\n\
sleep 5\n\
echo "Running database migrations..."\n\
cd /src/src/WareHouseManagement.API\n\
dotnet ef database update --no-build || echo "Migration failed, but continuing..."\n\
echo "Starting application..."\n\
cd /app\n\
exec dotnet WareHouseManagement.API.dll' > /app/entrypoint.sh && chmod +x /app/entrypoint.sh

EXPOSE 8080

ENTRYPOINT ["/bin/bash", "/app/entrypoint.sh"]

