# Warehouse Management System - Quick Start Script
# This script automatically starts the entire system in Docker

Write-Host "Starting Warehouse Management System..." -ForegroundColor Cyan
Write-Host ""

# Check if Docker is running
Write-Host "Checking Docker..." -ForegroundColor Yellow
try {
    docker --version | Out-Null
    docker-compose --version | Out-Null
    Write-Host "[OK] Docker is installed" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] Docker is not running. Please start Docker Desktop first." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Step 1: Starting PostgreSQL..." -ForegroundColor Yellow
docker-compose up -d postgres

Write-Host "Waiting for PostgreSQL to be ready (10 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

Write-Host ""
Write-Host "Step 2: Running database migrations..." -ForegroundColor Yellow
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"

try {
    dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API 2>&1 | Out-Null
    Write-Host "[OK] Database migrations completed successfully" -ForegroundColor Green
} catch {
    Write-Host "[WARNING] Migration warning (this might be OK if database already exists)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Step 3: Building and starting API..." -ForegroundColor Yellow
docker-compose build api 2>&1 | Out-Null
docker-compose up -d

Write-Host ""
Write-Host "Step 4: Checking container status..." -ForegroundColor Yellow
Start-Sleep -Seconds 3
docker-compose ps

Write-Host ""
Write-Host "[SUCCESS] Setup Complete!" -ForegroundColor Green
Write-Host ""
Write-Host "Services available at:" -ForegroundColor Cyan
Write-Host "   - API Swagger:  http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   - pgAdmin:      http://localhost:8080" -ForegroundColor White
Write-Host ""
Write-Host "pgAdmin credentials:" -ForegroundColor Cyan
Write-Host "   Email:    admin@admin.com" -ForegroundColor White
Write-Host "   Password: admin" -ForegroundColor White
Write-Host ""

# Wait a bit for API to be ready
Write-Host "Waiting for API to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# Open Swagger in browser
Write-Host "Opening Swagger UI in browser..." -ForegroundColor Yellow
Start-Process "http://localhost:5000/swagger"

Write-Host ""
Write-Host "[DONE] All done! Happy coding!" -ForegroundColor Green
Write-Host ""
Write-Host "Useful commands:" -ForegroundColor Cyan
Write-Host "   docker-compose logs -f          # View logs" -ForegroundColor White
Write-Host "   docker-compose stop             # Stop services" -ForegroundColor White
Write-Host "   docker-compose down             # Stop and remove" -ForegroundColor White
Write-Host "   docker-compose down -v          # Stop and remove with data" -ForegroundColor White
Write-Host ""

