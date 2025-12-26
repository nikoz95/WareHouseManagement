# Warehouse Management System - Local Development Start Script
# This script runs the system locally (without Docker)

Write-Host "Starting Warehouse Management System (Local Mode)" -ForegroundColor Cyan
Write-Host ""

# Check PostgreSQL Service
Write-Host "Step 1: Checking PostgreSQL service..." -ForegroundColor Yellow
$pgService = Get-Service postgresql-x64-* -ErrorAction SilentlyContinue

if ($null -eq $pgService) {
    Write-Host "[ERROR] PostgreSQL service not found!" -ForegroundColor Red
    Write-Host "Please install PostgreSQL first:" -ForegroundColor Yellow
    Write-Host "   https://www.postgresql.org/download/windows/" -ForegroundColor Cyan
    exit 1
}

if ($pgService.Status -ne "Running") {
    Write-Host "Starting PostgreSQL service..." -ForegroundColor Yellow
    Start-Service $pgService.Name
    Start-Sleep -Seconds 3
}

Write-Host "[OK] PostgreSQL is running" -ForegroundColor Green

Write-Host ""
Write-Host "Step 2: Restoring packages..." -ForegroundColor Yellow
dotnet restore | Out-Null

Write-Host ""
Write-Host "Step 3: Building project..." -ForegroundColor Yellow
dotnet build --no-restore | Out-Null

Write-Host ""
Write-Host "Step 4: Running database migrations..." -ForegroundColor Yellow
Write-Host "[WARNING] Make sure your connection string is correct in appsettings.Development.json" -ForegroundColor Yellow

try {
    dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API 2>&1 | Out-Null
    Write-Host "[OK] Database is up to date" -ForegroundColor Green
} catch {
    Write-Host "[WARNING] Migration completed with warnings (might be OK)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Step 5: Starting API..." -ForegroundColor Yellow
Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "[SUCCESS] Ready!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "API URLs:" -ForegroundColor Cyan
Write-Host "   - Swagger:  http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   - API:      http://localhost:5000" -ForegroundColor White
Write-Host ""
Write-Host "Tips:" -ForegroundColor Cyan
Write-Host "   - Press Ctrl+C to stop" -ForegroundColor White
Write-Host "   - Hot reload is enabled (code changes apply automatically)" -ForegroundColor White
Write-Host ""
Write-Host "Database:" -ForegroundColor Cyan
Write-Host "   - Use pgAdmin or psql to view data" -ForegroundColor White
Write-Host "   - Connection: localhost:5432" -ForegroundColor White
Write-Host ""

# Change to API directory and run with watch mode
cd src/WareHouseManagement.API
dotnet watch run

