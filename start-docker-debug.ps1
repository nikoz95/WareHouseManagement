# Start Docker containers in DEBUG mode
# This script starts the application with debugging support

Write-Host "🔧 Starting WareHouse Management in DEBUG mode..." -ForegroundColor Cyan
Write-Host ""

# Stop any existing containers
Write-Host "Stopping existing containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.debug.yml down 2>$null

# Build and start containers
Write-Host "Building and starting containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.debug.yml up --build -d

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Containers started successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📊 Services:" -ForegroundColor Cyan
    Write-Host "   - API:      http://localhost:5000" -ForegroundColor White
    Write-Host "   - Swagger:  http://localhost:5000/swagger" -ForegroundColor White
    Write-Host "   - pgAdmin:  http://localhost:8080 (admin@admin.com / admin)" -ForegroundColor White
    Write-Host "   - Debug:    Ready for attachment on port 5001" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "🐛 Debug Instructions:" -ForegroundColor Magenta
    Write-Host "   1. In Rider, go to Run > Attach to Process..." -ForegroundColor White
    Write-Host "   2. Select 'Docker' connection type" -ForegroundColor White
    Write-Host "   3. Choose 'warehouse_api_debug' container" -ForegroundColor White
    Write-Host "   4. Select the dotnet process" -ForegroundColor White
    Write-Host "   5. Set breakpoints and start debugging!" -ForegroundColor White
    Write-Host ""
    Write-Host "📝 View logs:" -ForegroundColor Cyan
    Write-Host "   docker-compose -f docker-compose.debug.yml logs -f api_debug" -ForegroundColor Gray
    Write-Host ""
    Write-Host "🛑 Stop containers:" -ForegroundColor Cyan
    Write-Host "   docker-compose -f docker-compose.debug.yml down" -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "❌ Failed to start containers" -ForegroundColor Red
    Write-Host "Check the error messages above for details" -ForegroundColor Yellow
    exit 1
}

