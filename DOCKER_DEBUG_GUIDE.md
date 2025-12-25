# Docker Debug Guide (როგორ გავაკეთოთ Debug Docker-იდან)

## 🐛 Debugging with Docker

### Option 1: Using Rider's Built-in Docker Debugging (Recommended)

#### Setup:
1. **Start containers in debug mode:**
   ```powershell
   .\start-docker-debug.ps1
   ```

2. **In Rider:**
   - Go to: `Run` → `Attach to Process...` (Ctrl+Alt+F5)
   - In the dialog:
     - **Connection:** Select `Docker`
     - **Container:** Select `warehouse_api_debug`
     - **Process:** Select the `dotnet` process
   - Click `OK`

3. **Set breakpoints:**
   - Open any file (e.g., `GetAllCompanyLocationsQueryHandler.cs`)
   - Click in the left margin to set breakpoints
   - Make an API request to trigger the breakpoint

4. **Test the API:**
   - Navigate to: http://localhost:5000/swagger
   - Execute any endpoint
   - Debugger will pause at your breakpoints

---

### Option 2: Using Remote Debugging with vsdbg

#### 1. Start Debug Containers:
```powershell
.\start-docker-debug.ps1
```

#### 2. Get Container ID:
```powershell
docker ps
# Find: warehouse_api_debug
```

#### 3. Attach vsdbg in Rider:
- `Run` → `Edit Configurations...`
- `Add New Configuration` → `.NET Remote`
- Configure:
  - **Name:** `Docker Debug`
  - **Host:** `localhost`
  - **Port:** `5001`
  - **Connection type:** `Remote`

---

### Option 3: Local Debugging (Easiest - PostgreSQL in Docker, API locally)

#### 1. Start only PostgreSQL:
```powershell
docker-compose -f docker-compose.postgres.yml up -d
```

#### 2. Run API locally in Rider:
- Press `F5` or click the green play button
- Set breakpoints normally
- Debug as usual

#### 3. Connection String:
Already configured in `appsettings.Development.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
}
```

---

## 📊 Debugging Specific Scenarios

### Debug Company Locations Query:

1. **Set breakpoint in:**
   ```
   GetAllCompanyLocationsQueryHandler.cs - line 17
   ```

2. **Test via Swagger:**
   - Go to: http://localhost:5000/swagger
   - Find: `GET /api/CompanyLocations`
   - Click "Try it out"
   - Execute
   - Debugger will pause

3. **Inspect variables:**
   - `request.CompanyId` - Check if filtering by company
   - `locationDtos` - See the results
   - `company.CompanyLocations` - Navigate collections

---

## 🔍 Common Debugging Commands

### View Logs:
```powershell
# All services
docker-compose -f docker-compose.debug.yml logs -f

# Only API
docker-compose -f docker-compose.debug.yml logs -f api_debug

# Only PostgreSQL
docker-compose -f docker-compose.debug.yml logs -f postgres
```

### Execute commands in container:
```powershell
# Open shell in API container
docker exec -it warehouse_api_debug /bin/bash

# Check if app is running
docker exec warehouse_api_debug ps aux

# View environment variables
docker exec warehouse_api_debug env
```

### Database debugging:
```powershell
# Connect to PostgreSQL
docker exec -it warehouse_postgres_debug psql -U warehouse_user -d WareHouseManagementDb

# Or use pgAdmin: http://localhost:8080
```

---

## 🛠️ Troubleshooting

### Problem: Can't attach debugger
**Solution:**
```powershell
# Rebuild containers
docker-compose -f docker-compose.debug.yml down
docker-compose -f docker-compose.debug.yml up --build -d
```

### Problem: Breakpoints not hitting
**Solutions:**
1. Make sure you're using `Dockerfile.debug` (not regular `Dockerfile`)
2. Verify source code matches running container
3. Check if optimization is disabled (Debug configuration)
4. Rebuild: `docker-compose -f docker-compose.debug.yml up --build -d`

### Problem: Port 5432 already in use
**Solution:**
```powershell
# Stop all containers
docker-compose down
docker-compose -f docker-compose.debug.yml down
docker-compose -f docker-compose.postgres.yml down

# Or change port in docker-compose.debug.yml
```

---

## 📝 Debugging Best Practices

1. **Use Option 3 (Local API + Docker PostgreSQL)** for fastest debugging
2. **Use Option 1 (Full Docker)** for testing production-like environment
3. **Set breakpoints before starting requests**
4. **Use Watch window** to monitor variables
5. **Use Immediate window** to execute code during debugging

---

## 🎯 Quick Start (Recommended Workflow)

```powershell
# 1. Start only PostgreSQL
docker-compose -f docker-compose.postgres.yml up -d

# 2. Run API in Rider (press F5)

# 3. Set breakpoints in your code

# 4. Test via Swagger: http://localhost:5000/swagger

# 5. Debug normally!
```

---

## 📞 Need Help?

- Check logs: `docker-compose -f docker-compose.debug.yml logs -f`
- Verify containers: `docker ps`
- Check health: `docker inspect warehouse_api_debug`

