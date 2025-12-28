# Health Check Test Script
# Tests the /health endpoint

$baseUrl = "http://localhost:5000"

Write-Host "`n====================================" -ForegroundColor Cyan
Write-Host "Testing WareHouse Management Health Check" -ForegroundColor Cyan
Write-Host "====================================`n" -ForegroundColor Cyan

# Test Health Check endpoint
Write-Host "Testing Health Check (/health)..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/health" -Method Get -ErrorAction Stop
    Write-Host "✅ Health Check: OK" -ForegroundColor Green
    Write-Host "   Response: $response" -ForegroundColor Gray
    Write-Host "   API და Database მუშაობს!" -ForegroundColor Green
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host "❌ Health Check Failed" -ForegroundColor Red
    Write-Host "   Status Code: $statusCode" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($statusCode -eq 503) {
        Write-Host "   Database კავშირის პრობლემა!" -ForegroundColor Yellow
    }
}

Write-Host "`n====================================" -ForegroundColor Cyan
Write-Host "Health Check Test Complete!" -ForegroundColor Cyan
Write-Host "====================================`n" -ForegroundColor Cyan



