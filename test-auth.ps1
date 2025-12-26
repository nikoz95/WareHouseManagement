# JWT Authentication Test Script
# ეს სკრიპტი აჩვენებს როგორ შეამოწმო JWT Authentication

$apiUrl = "http://localhost:5000/api"

Write-Host "🔐 JWT Authentication ტესტირება" -ForegroundColor Cyan
Write-Host "=" * 50

# Test 1: Login როგორც Admin
Write-Host "`n1️⃣ Admin-ით შესვლა..." -ForegroundColor Yellow
$loginBody = @{
    username = "admin"
    password = "Admin123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$apiUrl/auth/login" -Method Post -Body $loginBody -ContentType "application/json"
    Write-Host "✅ წარმატებული ავტორიზაცია!" -ForegroundColor Green
    Write-Host "Access Token: $($loginResponse.accessToken.Substring(0, 50))..." -ForegroundColor Gray
    Write-Host "Refresh Token: $($loginResponse.refreshToken.Substring(0, 50))..." -ForegroundColor Gray
    Write-Host "User: $($loginResponse.user.username) - Roles: $($loginResponse.user.roles -join ', ')" -ForegroundColor Cyan
    
    $adminToken = $loginResponse.accessToken
    $adminRefreshToken = $loginResponse.refreshToken
} catch {
    Write-Host "❌ შეცდომა: $($_.Exception.Message)" -ForegroundColor Red
    exit
}

# Test 2: Protected endpoint - Admin only
Write-Host "`n2️⃣ Admin endpoint-ის ტესტირება..." -ForegroundColor Yellow
try {
    $headers = @{
        "Authorization" = "Bearer $adminToken"
    }
    $adminResponse = Invoke-RestMethod -Uri "$apiUrl/auth/test-admin" -Method Get -Headers $headers
    Write-Host "✅ $($adminResponse.message)" -ForegroundColor Green
} catch {
    Write-Host "❌ შეცდომა: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Login როგორც Guest
Write-Host "`n3️⃣ Guest-ით შესვლა..." -ForegroundColor Yellow
$guestLoginBody = @{
    username = "guest"
    password = "Guest123!"
} | ConvertTo-Json

try {
    $guestLoginResponse = Invoke-RestMethod -Uri "$apiUrl/auth/login" -Method Post -Body $guestLoginBody -ContentType "application/json"
    Write-Host "✅ წარმატებული ავტორიზაცია!" -ForegroundColor Green
    Write-Host "User: $($guestLoginResponse.user.username) - Roles: $($guestLoginResponse.user.roles -join ', ')" -ForegroundColor Cyan
    
    $guestToken = $guestLoginResponse.accessToken
} catch {
    Write-Host "❌ შეცდომა: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Guest trying to access Admin endpoint
Write-Host "`n4️⃣ Guest-ი ცდილობს Admin endpoint-ზე წვდომას..." -ForegroundColor Yellow
try {
    $headers = @{
        "Authorization" = "Bearer $guestToken"
    }
    $response = Invoke-RestMethod -Uri "$apiUrl/auth/test-admin" -Method Get -Headers $headers
    Write-Host "❌ Guest-მა მიიღო წვდომა Admin endpoint-ზე (არასწორია!)" -ForegroundColor Red
} catch {
    Write-Host "✅ წვდომა აკრძალულია (სწორია!) - Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Green
}

# Test 5: Guest accessing Guest endpoint
Write-Host "`n5️⃣ Guest accessing Guest endpoint..." -ForegroundColor Yellow
try {
    $headers = @{
        "Authorization" = "Bearer $guestToken"
    }
    $response = Invoke-RestMethod -Uri "$apiUrl/auth/test-guest" -Method Get -Headers $headers
    Write-Host "✅ $($response.message)" -ForegroundColor Green
} catch {
    Write-Host "❌ შეცდომა: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 6: Refresh Token
Write-Host "`n6️⃣ Refresh Token-ის ტესტირება..." -ForegroundColor Yellow
$refreshBody = @{
    refreshToken = $adminRefreshToken
} | ConvertTo-Json

try {
    $refreshResponse = Invoke-RestMethod -Uri "$apiUrl/auth/refresh-token" -Method Post -Body $refreshBody -ContentType "application/json"
    Write-Host "✅ ახალი Access Token მიღებულია!" -ForegroundColor Green
    Write-Host "New Access Token: $($refreshResponse.accessToken.Substring(0, 50))..." -ForegroundColor Gray
} catch {
    Write-Host "❌ შეცდომა: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 7: Permission-based endpoint
Write-Host "`n7️⃣ Permission-based endpoint ტესტირება (Product.Read)..." -ForegroundColor Yellow
try {
    $headers = @{
        "Authorization" = "Bearer $adminToken"
    }
    $response = Invoke-RestMethod -Uri "$apiUrl/auth/test-permission" -Method Get -Headers $headers
    Write-Host "✅ $($response.message)" -ForegroundColor Green
} catch {
    Write-Host "❌ შეცდომა: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n" + ("=" * 50)
Write-Host "✅ ტესტირება დასრულდა!" -ForegroundColor Cyan
Write-Host "`nშენახული Tokens:" -ForegroundColor Yellow
Write-Host "Admin Token: `$adminToken" -ForegroundColor Gray
Write-Host "Guest Token: `$guestToken" -ForegroundColor Gray
Write-Host "`nეს tokens შეგიძლია გამოიყენო Swagger-ში ან Postman-ში!" -ForegroundColor Cyan

