# 🐛 Docker Debug Setup - Summary

## ✅ რა შევქმენით:

### 1. **Dockerfile.debug** - Debug-enabled Docker image
   - შეიცავს .NET SDK-ს (არა მხოლოდ runtime)
   - დაინსტალირებული vsdbg debugger-ით
   - Source code-ს აკოპირებს Debug mode-ით build-ისთვის
   - Hot reload support

### 2. **docker-compose.debug.yml** - Debug configuration
   - PostgreSQL container
   - API container Debug mode-ით
   - pgAdmin container
   - Debug port (5001) expose-ბული
   - Source code volume mount (hot reload-ისთვის)
   - Security settings debugger-ისთვის

### 3. **start-docker-debug.ps1** - Script debug containers-ის გასაშვებად
   - Automatically stops old containers
   - Builds and starts debug containers
   - Shows all service URLs
   - Shows debugging instructions

### 4. **DOCKER_DEBUG_GUIDE.md** - Complete debugging guide
   - 3 debug option-ის აღწერა
   - Step-by-step instructions
   - Troubleshooting tips
   - Common commands

---

## 🚀 როგორ გამოვიყენოთ:

### Option 1: უმარტივესი (რეკომენდებული)
```powershell
# 1. Start only PostgreSQL in Docker
docker-compose -f docker-compose.postgres.yml up -d

# 2. Run API in Rider (press F5)

# 3. Set breakpoints and debug normally
```

### Option 2: Full Docker Debugging
```powershell
# 1. Start debug containers
.\start-docker-debug.ps1

# 2. In Rider: Run → Attach to Process...
#    - Connection: Docker
#    - Container: warehouse_api_debug
#    - Process: dotnet

# 3. Set breakpoints and debug
```

### Option 3: Remote Debugging
```powershell
# 1. Start debug containers
.\start-docker-debug.ps1

# 2. Configure .NET Remote in Rider
#    Host: localhost, Port: 5001

# 3. Start debugging session
```

---

## 📁 ფაილების აღწერა:

```
WareHouseManagment/
├── Dockerfile.debug              # Debug-enabled Dockerfile
├── docker-compose.debug.yml      # Debug services configuration
├── start-docker-debug.ps1        # Quick start script for debugging
├── DOCKER_DEBUG_GUIDE.md         # Complete debugging guide
├── Dockerfile                    # Production Dockerfile (unchanged)
├── docker-compose.yml            # Production compose (unchanged)
└── docker-compose.postgres.yml   # PostgreSQL only (for local dev)
```

---

## 🎯 რა პრობლემებს წყვეტს:

1. ✅ **Docker-იდან debugging** - ახლა შეგიძლიათ breakpoint-ების დასმა Docker containers-ში
2. ✅ **Remote debugging** - attach debugger-ის საშუალება remote containers-ზე
3. ✅ **Hot reload** - source code changes automatically reflected
4. ✅ **Production-like testing** - debug production-like environment-ში
5. ✅ **Isolate issues** - ადვილად გაარკვევთ Docker-specific პრობლემები

---

## 🔧 გამოყენებული ტექნოლოგიები:

- **vsdbg** - Visual Studio debugger for .NET
- **Docker SDK image** - .NET 9.0 SDK (არა runtime-only)
- **SYS_PTRACE** - Linux capability debugger-ისთვის
- **Volume mounts** - Source code hot reload-ისთვის
- **Port forwarding** - Debug port exposure

---

## 📝 შემდეგი ნაბიჯები:

1. **წაიკითხეთ:** [DOCKER_DEBUG_GUIDE.md](./DOCKER_DEBUG_GUIDE.md)
2. **გამოცადეთ:** Option 1 (Local API + Docker PostgreSQL)
3. **თუ სჭირდება:** Full Docker debugging → Option 2
4. **Advanced:** Remote debugging setup → Option 3

---

## 🆘 დახმარება:

თუ პრობლემები გაქვთ:
1. შეამოწმეთ [DOCKER_DEBUG_GUIDE.md](./DOCKER_DEBUG_GUIDE.md) - Troubleshooting section
2. გაუშვით: `docker-compose -f docker-compose.debug.yml logs -f`
3. Verify: `docker ps` - ყველა container running-ია?

---

**გაითვალისწინეთ:** Debug mode უფრო ნელია production mode-ზე, რადგან:
- SDK image უფრო დიდია
- Debug symbols-ები კომპილირებულია
- Optimization disabled-ია
- Source maps active-ია

**Production-ისთვის** გამოიყენეთ ჩვეულებრივი `docker-compose.yml` და `Dockerfile`!

