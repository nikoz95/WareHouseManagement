# Health Check Endpoints

## Overview
სისტემის ჯანმრთელობის შემოწმების ენდფოინთები (Health Check Endpoints) საშუალებას გაძლევთ შეამოწმოთ API-ის და მონაცემთა ბაზის მდგომარეობა.

## Available Endpoints

### 1. Simple Health Check
**Endpoint:** `GET /health`  
**Authorization:** Not required (Anonymous)  
**Description:** მარტივი ჯანმრთელობის შემოწმება. აბრუნებს ASP.NET Core-ის ჩაშენებულ health check-ს.

**Example Request:**
```bash
curl http://localhost:5000/health
```

**Example Response:**
```
Healthy
```

---

### 2. Basic Health Check
**Endpoint:** `GET /api/health`  
**Authorization:** Not required (Anonymous)  
**Description:** მარტივი ჯანმრთელობის შემოწმება JSON ფორმატში.

**Example Request:**
```bash
curl http://localhost:5000/api/health
```

**Example Response:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-12-28T10:30:00Z",
  "message": "WareHouse Management API is running"
}
```

---

### 3. Detailed Health Check
**Endpoint:** `GET /api/health/detailed`  
**Authorization:** Not required (Anonymous)  
**Description:** დეტალური ჯანმრთელობის შემოწმება API-ისა და მონაცემთა ბაზის მდგომარეობის ინფორმაციით.

**Example Request:**
```bash
curl http://localhost:5000/api/health/detailed
```

**Example Response:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-12-28T10:30:00Z",
  "service": "WareHouse Management API",
  "version": "1.0.0",
  "checks": {
    "api": {
      "status": "Healthy",
      "message": "API is running"
    },
    "database": {
      "status": "Healthy",
      "message": "Database connection successful",
      "timestamp": "2024-12-28T10:30:00Z",
      "details": {
        "connectionState": "Connected",
        "productCount": 5
      }
    }
  }
}
```

---

### 4. Database Health Check
**Endpoint:** `GET /api/health/database`  
**Authorization:** Not required (Anonymous)  
**Description:** მხოლოდ მონაცემთა ბაზის ჯანმრთელობის შემოწმება.

**Example Request:**
```bash
curl http://localhost:5000/api/health/database
```

**Example Response (Healthy):**
```json
{
  "status": "Healthy",
  "message": "Database connection successful",
  "timestamp": "2024-12-28T10:30:00Z",
  "details": {
    "connectionState": "Connected",
    "productCount": 5
  }
}
```

**Example Response (Unhealthy):**
```json
{
  "status": "Unhealthy",
  "message": "Database health check failed",
  "error": "Connection timeout",
  "timestamp": "2024-12-28T10:30:00Z"
}
```
**HTTP Status:** 503 Service Unavailable (თუ database არ არის available)

---

## Testing Health Checks

### Using PowerShell Script
პროექტში არის test-health.ps1 სკრიპტი, რომელიც ყველა health check-ს ამოწმებს:

```powershell
.\test-health.ps1
```

### Using Postman
Postman Collection-ში (`WareHouse_Complete_Flow.postman_collection.json`) დამატებულია 3 health check request:
- **0. Health Check - Simple** - `/health`
- **0. Health Check - Detailed** - `/api/health/detailed`
- **0. Health Check - Database** - `/api/health/database`

### Using curl
```bash
# Simple check
curl http://localhost:5000/health

# Basic JSON check
curl http://localhost:5000/api/health

# Detailed check
curl http://localhost:5000/api/health/detailed

# Database check
curl http://localhost:5000/api/health/database
```

## Use Cases

### 1. Container Orchestration (Kubernetes, Docker Swarm)
```yaml
livenessProbe:
  httpGet:
    path: /health
    port: 5000
  initialDelaySeconds: 30
  periodSeconds: 10

readinessProbe:
  httpGet:
    path: /api/health/database
    port: 5000
  initialDelaySeconds: 5
  periodSeconds: 5
```

### 2. Load Balancer Health Checks
Health check ენდფოინთები შეიძლება გამოყენებულ იქნას Load Balancer-ებისთვის (nginx, HAProxy, AWS ELB):
- `/health` - quick check
- `/api/health/database` - deep check with DB connectivity

### 3. Monitoring Tools
Integration with monitoring tools like:
- Prometheus
- Grafana
- New Relic
- Datadog
- Azure Application Insights

### 4. CI/CD Pipelines
```bash
# Wait for API to be healthy before running tests
until curl -f http://localhost:5000/health; do
  echo "Waiting for API..."
  sleep 2
done
echo "API is healthy, running tests..."
```

## Status Codes

| Endpoint | Healthy Status | Unhealthy Status |
|----------|---------------|------------------|
| `/health` | 200 OK | 503 Service Unavailable |
| `/api/health` | 200 OK | N/A (always returns 200) |
| `/api/health/detailed` | 200 OK | N/A (always returns 200) |
| `/api/health/database` | 200 OK | 503 Service Unavailable |

## Implementation Details

### Built-in ASP.NET Core Health Checks
Program.cs-ში დამატებულია:
```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database");

app.MapHealthChecks("/health");
```

### Custom Health Controller
`HealthController.cs` იძლევა მეტ კონტროლს health check-ების რესპონსებზე:
- Custom JSON responses
- Detailed diagnostics
- Database query testing
- Error handling and logging

## Security Considerations

⚠️ **Important:** ყველა health check endpoint არის anonymous (არ საჭიროებს ავტორიზაციას).

**Recommendations:**
1. მონიტორინგის სისტემებისთვის გამოიყენეთ `/health` (მარტივი, სწრაფი)
2. დეტალური ინფორმაციისთვის (`/api/health/detailed`) შესაძლოა დაგჭირდეთ authorization-ის დამატება production-ში
3. არ გამოაჩინოთ sensitive information health check responses-ში

