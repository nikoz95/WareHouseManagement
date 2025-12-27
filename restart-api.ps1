#!/usr/bin/env pwsh
# API Restart Script - Publish, Build, and Replace API Container Only

Write-Host "Starting API Restart Process..." -ForegroundColor Cyan
Write-Host ""

# Step 1: Publish API
Write-Host "Step 1: Publishing API..." -ForegroundColor Yellow
Set-Location -Path $PSScriptRoot

dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Publish failed!" -ForegroundColor Red
    exit 1
}

Write-Host "SUCCESS: Publish completed!" -ForegroundColor Green
Write-Host ""

# Step 2: Stop API container
Write-Host "Step 2: Stopping API container..." -ForegroundColor Yellow
docker-compose stop api
docker-compose rm -f api

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Docker-compose stop failed!" -ForegroundColor Red
    exit 1
}

Write-Host "SUCCESS: API container stopped!" -ForegroundColor Green
Write-Host ""

# Step 3: Start API container (will rebuild with new publish)
Write-Host "Step 3: Starting API container..." -ForegroundColor Yellow
docker-compose up -d --build api

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Container startup failed!" -ForegroundColor Red
    exit 1
}

Write-Host "SUCCESS: API container started!" -ForegroundColor Green
Write-Host ""

# Step 4: Show container status
Write-Host "Container Status:" -ForegroundColor Cyan
docker-compose ps

Write-Host ""
Write-Host "API Restart Complete!" -ForegroundColor Green
Write-Host "   API is running at: http://localhost:5000" -ForegroundColor Cyan
Write-Host "   Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "View logs with: docker-compose logs -f api" -ForegroundColor Gray

