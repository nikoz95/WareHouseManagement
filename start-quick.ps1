# Quick Start Script - PostgreSQL Only
# This starts just PostgreSQL in Docker, then runs the API locally

Write-Host "🚀 Starting PostgreSQL in Docker..." -ForegroundColor Green
Write-Host ""

# Check if Docker is running
$dockerRunning = $false
try {
    $null = docker ps 2>&1
    $dockerRunning = $true
    Write-Host "✓ Docker is running" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker Desktop is not running." -ForegroundColor Red
    Write-Host "Please start Docker Desktop and run this script again." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Press any key to exit..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

# Start PostgreSQL
Write-Host ""
Write-Host "🐘 Starting PostgreSQL container..." -ForegroundColor Yellow
docker-compose -f docker-compose.postgres.yml up -d

# Wait for PostgreSQL to be ready
Write-Host ""
Write-Host "⏳ Waiting for PostgreSQL to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

$maxAttempts = 30
$attempt = 0
$ready = $false

while ($attempt -lt $maxAttempts -and -not $ready) {
    try {
        $result = docker exec warehouse_postgres pg_isready -U warehouse_user -d WareHouseManagementDb 2>&1
        if ($result -match "accepting connections") {
            $ready = $true
        }
    } catch {
        # Container not ready yet
    }
    
    if (-not $ready) {
        Write-Host "." -NoNewline
        Start-Sleep -Seconds 1
        $attempt++
    }
}

Write-Host ""
if ($ready) {
    Write-Host "✓ PostgreSQL is ready!" -ForegroundColor Green
} else {
    Write-Host "⚠ PostgreSQL may still be starting. Please wait a moment." -ForegroundColor Yellow
}

# Update connection string in appsettings
Write-Host ""
Write-Host "📝 Updating connection string..." -ForegroundColor Yellow
$appsettingsPath = "src\WareHouseManagement.API\appsettings.Development.json"
$connectionString = "Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"

$appsettingsContent = @"
{
  "ConnectionStrings": {
    "DefaultConnection": "$connectionString"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
"@

Set-Content -Path $appsettingsPath -Value $appsettingsContent
Write-Host "✓ Connection string updated" -ForegroundColor Green

# Run migrations
Write-Host ""
Write-Host "🔄 Running database migrations..." -ForegroundColor Yellow
cd src\WareHouseManagement.API
dotnet ef database update

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Migrations completed successfully!" -ForegroundColor Green
} else {
    Write-Host "⚠ Migration failed. You may need to run it manually." -ForegroundColor Yellow
}

# Ask if user wants to start the API
Write-Host ""
Write-Host "✅ PostgreSQL is running!" -ForegroundColor Green
Write-Host ""
Write-Host "📊 Database Info:" -ForegroundColor Cyan
Write-Host "   Host: localhost" -ForegroundColor White
Write-Host "   Port: 5432" -ForegroundColor White
Write-Host "   Database: WareHouseManagementDb" -ForegroundColor White
Write-Host "   Username: warehouse_user" -ForegroundColor White
Write-Host "   Password: warehouse_pass_2024" -ForegroundColor White
Write-Host ""
Write-Host "Do you want to start the API now? (Y/N): " -NoNewline -ForegroundColor Yellow
$response = Read-Host

if ($response -eq 'Y' -or $response -eq 'y') {
    Write-Host ""
    Write-Host "🚀 Starting API..." -ForegroundColor Green
    dotnet run
} else {
    Write-Host ""
    Write-Host "To start the API later, run:" -ForegroundColor Cyan
    Write-Host "   cd src\WareHouseManagement.API" -ForegroundColor White
    Write-Host "   dotnet run" -ForegroundColor White
    Write-Host ""
    Write-Host "To stop PostgreSQL:" -ForegroundColor Cyan
    Write-Host "   docker-compose -f docker-compose.postgres.yml down" -ForegroundColor White
    Write-Host ""
}

