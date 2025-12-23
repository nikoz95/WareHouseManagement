# WareHouse Management System - Docker Setup
```
docker-compose up -d
docker-compose down -v
```powershell
### Reset database

Wait a few seconds for PostgreSQL to fully start before the API tries to connect. The healthcheck ensures this happens automatically.
### Database not connecting

- For API: Change `"5000:8080"` to `"5001:8080"`
- For PostgreSQL: Change `"5432:5432"` to `"5433:5432"`
If port 5432 or 5000 is already in use, you can change them in `docker-compose.yml`:
### Port already in use

## Troubleshooting

- **Password**: warehouse_pass_2024
- **Username**: warehouse_user
- **Database**: WareHouseManagementDb
- **Port**: 5432
- **Host**: localhost

## Database Connection Details

```
docker-compose exec postgres psql -U warehouse_user -d WareHouseManagementDb
# Access PostgreSQL directly

docker-compose up --build
# Rebuild containers

docker-compose down -v
# Stop and remove volumes (deletes database data)

docker-compose down
# Stop all services

docker-compose logs -f api
# View API logs only

docker-compose logs -f
# View logs
```powershell

## Useful Commands

```
docker-compose exec api dotnet ef database update --project /src/src/WareHouseManagement.Infrastructure
# If running in Docker

dotnet ef database update
cd src\WareHouseManagement.API
# If running locally
```powershell
To manually run migrations:

Migrations are automatically applied when the API container starts. 

## Database Migrations

```
dotnet run
cd src\WareHouseManagement.API
# Then run the API locally

docker-compose up postgres -d
# Start only PostgreSQL
```powershell
### Option 2: Run Only PostgreSQL in Docker

Swagger UI: http://localhost:5000/swagger
The API will be available at: http://localhost:5000

```
docker-compose up -d --build
# Or run in detached mode

docker-compose up --build
# Build and start all services (PostgreSQL + API)
```powershell
### Option 1: Run Everything with Docker Compose

## Quick Start

- Docker Desktop installed and running
## Prerequisites


