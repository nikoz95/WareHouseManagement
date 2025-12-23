@echo off
echo.
echo ========================================
echo   WareHouse Management System
echo   Docker Setup
echo ========================================
echo.
echo Starting Docker services...
echo.

docker-compose -f docker-compose.postgres.yml up -d

echo.
echo Waiting for PostgreSQL to be ready...
timeout /t 10 /nobreak >nul

echo.
echo Running database migrations...
cd src\WareHouseManagement.API
dotnet ef database update

echo.
echo.
echo ========================================
echo   Setup Complete!
echo ========================================
echo.
echo PostgreSQL is running at: localhost:5432
echo Database: WareHouseManagementDb
echo.
echo To start the API:
echo   cd src\WareHouseManagement.API
echo   dotnet run
echo.
echo To stop PostgreSQL:
echo   docker-compose -f docker-compose.postgres.yml down
echo.
echo ========================================
echo.
pause

