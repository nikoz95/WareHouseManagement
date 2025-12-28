# Health Check Test Script
# Tests all health check endpoints

$baseUrl = "http://localhost:5000"

Write-Host "`n====================================" -ForegroundColor Cyan
Write-Host "Testing WareHouse Management Health Checks" -ForegroundColor Cyan
Write-Host "====================================`n" -ForegroundColor Cyan

# Test 1: Simple Health Check (built-in endpoint)
Write-Host "1. Testing Simple Health Check (/health)..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/health" -Method Get -ErrorAction Stop
    Write-Host "✅ Simple Health Check: OK" -ForegroundColor Green
    Write-Host "   Response: $response" -ForegroundColor Gray
} catch {
    Write-Host "❌ Simple Health Check Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 2: Custom Health Check - Basic
Write-Host "2. Testing Custom Health Check (/api/health)..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/health" -Method Get -ErrorAction Stop
    Write-Host "✅ Custom Health Check: OK" -ForegroundColor Green
    Write-Host "   Status: $($response.status)" -ForegroundColor Gray
    Write-Host "   Message: $($response.message)" -ForegroundColor Gray
    Write-Host "   Timestamp: $($response.timestamp)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Custom Health Check Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 3: Detailed Health Check
Write-Host "3. Testing Detailed Health Check (/api/health/detailed)..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/health/detailed" -Method Get -ErrorAction Stop
    Write-Host "✅ Detailed Health Check: OK" -ForegroundColor Green
    Write-Host "   Service: $($response.service)" -ForegroundColor Gray
    Write-Host "   Version: $($response.version)" -ForegroundColor Gray
    Write-Host "   API Status: $($response.checks.api.status)" -ForegroundColor Gray
    Write-Host "   Database Status: $($response.checks.database.status)" -ForegroundColor Gray
    if ($response.checks.database.details) {
        Write-Host "   Product Count: $($response.checks.database.details.productCount)" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Detailed Health Check Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 4: Database Health Check
Write-Host "4. Testing Database Health Check (/api/health/database)..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/health/database" -Method Get -ErrorAction Stop
    Write-Host "✅ Database Health Check: OK" -ForegroundColor Green
    Write-Host "   Status: $($response.status)" -ForegroundColor Gray
    Write-Host "   Message: $($response.message)" -ForegroundColor Gray
    if ($response.details) {
        Write-Host "   Connection State: $($response.details.connectionState)" -ForegroundColor Gray
        Write-Host "   Product Count: $($response.details.productCount)" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Database Health Check Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n====================================" -ForegroundColor Cyan
Write-Host "Health Check Tests Complete!" -ForegroundColor Cyan
Write-Host "====================================`n" -ForegroundColor Cyan

