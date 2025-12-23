# Docker Run Script
# This script builds and runs the WareHouse Management system with Docker

Write-Host "🚀 Starting WareHouse Management System with Docker..." -ForegroundColor Green
Write-Host ""

# Check if Docker is running
try {
    docker info | Out-Null
    Write-Host "✓ Docker is running" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker is not running. Please start Docker Desktop first." -ForegroundColor Red
    exit 1
}

# Stop any existing containers
Write-Host ""
Write-Host "🛑 Stopping existing containers..." -ForegroundColor Yellow
docker-compose down

# Build and start services
Write-Host ""
Write-Host "🔨 Building and starting services..." -ForegroundColor Yellow
docker-compose up --build -d

# Wait for services to be ready
Write-Host ""
Write-Host "⏳ Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Check service status
Write-Host ""
Write-Host "📊 Service Status:" -ForegroundColor Cyan
docker-compose ps

Write-Host ""
Write-Host "✅ Setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 API is available at: http://localhost:5000" -ForegroundColor Cyan
Write-Host "📚 Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "📝 View logs with: docker-compose logs -f" -ForegroundColor Yellow
Write-Host "🛑 Stop services with: docker-compose down" -ForegroundColor Yellow
Write-Host ""

