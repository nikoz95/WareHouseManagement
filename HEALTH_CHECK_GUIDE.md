# Health Check Endpoint

## Overview
სისტემის ჯანმრთელობის შემოწმების ენდფოინთი (Health Check Endpoint) საშუალებას გაძლევთ შეამოწმოთ API-ის მდგომარეობა.

## Available Endpoint

### Health Check
**Endpoint:** `GET /health`  
**Authorization:** Not required (Anonymous)  
**Description:** API-ის ჯანმრთელობის შემოწმება. ამოწმებს მხოლოდ API-ის availability-ს.

**Example Request:**
```bash
curl http://localhost:5000/health
```

**Example Response (Healthy):**
```
Healthy
```

**HTTP Status Code:**
- `200 OK` - API მუშაობს

---

## Testing Health Check

### Using PowerShell Script
პროექტში არის test-health.ps1 სკრიპტი, რომელიც health check-ს ამოწმებს:

```powershell
.\test-health.ps1
```

### Using Postman
Postman Collection-ში (`WareHouse_Complete_Flow.postman_collection.json`) დამატებულია health check request:
- **0. Health Check** - `/health`

### Using curl
```bash
curl http://localhost:5000/health
```

### Using PowerShell
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/health" -Method Get
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
    path: /health
    port: 5000
  initialDelaySeconds: 5
  periodSeconds: 5
```

### 2. Load Balancer Health Checks
Health check ენდფოინთი შეიძლება გამოყენებულ იქნას Load Balancer-ებისთვის (nginx, HAProxy, AWS ELB):
- `/health` - ამოწმებს API-ს და Database კავშირს

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

| Status | HTTP Code | Description |
|--------|-----------|-------------|
| Healthy | 200 OK | API და Database მუშაობს |
| Unhealthy | 503 Service Unavailable | Database კავშირის პრობლემა |

## Implementation Details

### ASP.NET Core Health Checks
Program.cs-ში დამატებულია:
```csharp
// Add Health Checks with database connectivity check
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database");

// Map health check endpoint
app.MapHealthChecks("/health");
```

ეს იმპლემენტაცია:
- ამოწმებს API-ის availability-ს
- ამოწმებს Database კავშირს (ApplicationDbContext)
- აბრუნებს "Healthy" ან "Unhealthy" status-ს

## Security Considerations

⚠️ **Important:** Health check endpoint არის anonymous (არ საჭიროებს ავტორიზაციას).

**Recommendations:**
1. `/health` endpoint უნდა იყოს anonymous რათა Load Balancers და Monitoring tools-მა შეძლოს მისი გამოყენება
2. არ გამოაჩინოთ sensitive information health check response-ში
3. Production-ში შეგიძლიათ დაამატოთ IP whitelist თუ გსურთ მხოლოდ კონკრეტული monitoring servers-ის access

