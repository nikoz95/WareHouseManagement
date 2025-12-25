# 🐳 Docker-ით აპლიკაციის გაშვება

## 📋 შინაარსი
- [წინაპირობები](#-წინაპირობები)
- [სწრაფი გაშვება](#-სწრაფი-გაშვება)
- [დეტალური ინსტრუქცია](#-დეტალური-ინსტრუქცია)
- [კონფიგურაცია](#-კონფიგურაცია)
- [Docker-ის მართვა](#-docker-ის-მართვა)
- [ხშირი პრობლემები](#-ხშირი-პრობლემები)

---

## ✅ წინაპირობები

დარწმუნდით რომ თქვენს კომპიუტერზე დაინსტალირებულია:

1. **Docker Desktop** (Windows/Mac) ან **Docker Engine** (Linux)
   - [Download Docker Desktop](https://www.docker.com/products/docker-desktop/)
   - გაშვებული უნდა იყოს Docker დაწყებამდე!

2. **.NET 9.0 SDK** (მიგრაციებისთვის - optional)
   - [Download .NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)

---

## ⚡ სწრაფი გაშვება

### PowerShell Script (რეკომენდებული)

```powershell
.\start-docker.ps1
```

ეს სკრიპტი ავტომატურად:
1. ✅ ამოწმებს Docker-ს
2. 🚀 გააშვებს PostgreSQL-ს
3. 📦 შექმნის ბაზას და ცხრილებს
4. 🏗️ დაბილდავს API-ს
5. 🎯 გაუშვებს ყველა სერვისს

### ხელით (Step by Step)

```powershell
# 1. PostgreSQL-ის გაშვება
docker-compose up -d postgres

# 2. დაელოდეთ 10 წამი
Start-Sleep -Seconds 10

# 3. ბაზის მიგრაციები (ცხრილების შექმნა)
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 4. API-ს build და გაშვება
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained
docker-compose build --no-cache
docker-compose up -d
```

---

## 📖 დეტალური ინსტრუქცია

### 1️⃣ პირველი გაშვება (Initial Setup)

#### ნაბიჯი 1: Docker-ის შემოწმება

```powershell
# Docker-ის ვერსიის შემოწმება
docker --version
docker-compose --version

# Docker-ის სტატუსი
docker info
```

თუ Docker არ მუშაობს:
- Windows: გახსენით Docker Desktop
- შეამოწმეთ Windows PowerShell-ს Administrator უფლებები

#### ნაბიჯი 2: PostgreSQL-ის გაშვება

```powershell
# PostgreSQL კონტეინერის გაშვება
docker-compose up -d postgres

# შემოწმება რომ გაშვდა
docker ps | Select-String postgres
```

მოცდა 10-15 წამი PostgreSQL-ის სრულად ჩასატვირთად:

```powershell
Start-Sleep -Seconds 10
```

#### ნაბიჯი 3: Database Migration

```powershell
# Connection String-ის დაყენება
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"

# Migration-ების გაშვება
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

ეს შექმნის:
- ✅ ყველა ცხრილს
- ✅ ინდექსებს
- ✅ Seed Data-ს (demo მონაცემები)

#### ნაბიჯი 4: აპლიკაციის Build

```powershell
# Clean build
dotnet clean
dotnet build

# Publish (Docker-ისთვის)
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained
```

#### ნაბიჯი 5: Docker Build & Run

```powershell
# Build Docker image
docker-compose build --no-cache

# გაშვება
docker-compose up -d
```

#### ნაბიჯი 6: შემოწმება

```powershell
# კონტეინერების სტატუსი
docker-compose ps

# Logs
docker-compose logs -f api
```

---

### 2️⃣ შემდგომი გაშვებები

```powershell
# უბრალოდ გაუშვით
docker-compose up -d

# ან თუ კოდი შეცვალეთ
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained
docker-compose build --no-cache
docker-compose up -d
```

---

## 🌐 ხელმისაწვდომი სერვისები

| სერვისი | URL | მიღება |
|---------|-----|--------|
| **API (Swagger)** | http://localhost:5000/swagger | API დოკუმენტაცია და ტესტირება |
| **pgAdmin** | http://localhost:8080 | PostgreSQL მართვა (UI) |
| **PostgreSQL** | localhost:5432 | პირდაპირი წვდომა (DBeaver, etc.) |

### Swagger API Testing

1. გახსენით: http://localhost:5000/swagger
2. აირჩიეთ endpoint
3. დააჭირეთ "Try it out"
4. შეიყვანეთ პარამეტრები
5. დააჭირეთ "Execute"

---

## 🔐 კონფიგურაცია

### pgAdmin Setup

1. **გახსენით:** http://localhost:8080

2. **შედით:**
   - Email: `admin@admin.com`
   - Password: `admin`

3. **დაამატეთ Server:**
   - Right-click "Servers" → Create → Server
   
   **General Tab:**
   - Name: `Warehouse Server`
   
   **Connection Tab:**
   - Host: `postgres` (⚠️ არა localhost!)
   - Port: `5432`
   - Database: `WareHouseManagementDb`
   - Username: `warehouse_user`
   - Password: `warehouse_pass_2024`
   
   - ✅ Save password: ჩართეთ

4. **დააჭირეთ Save**

---

## 🛠️ Docker-ის მართვა

### კონტეინერების მართვა

```powershell
# ყველა კონტეინერის სტატუსი
docker-compose ps

# ყველა კონტეინერის გაშვება
docker-compose up -d

# გაჩერება (კონტეინერები რჩება)
docker-compose stop

# გამორთვა (კონტეინერები იშლება)
docker-compose down

# გამორთვა + ბაზის წაშლა (ყველაფრის წაშლა)
docker-compose down -v
```

### კონკრეტული სერვისის მართვა

```powershell
# მხოლოდ API-ს რესტარტი
docker-compose restart api

# მხოლოდ PostgreSQL-ის რესტარტი
docker-compose restart postgres

# API-ს გაჩერება
docker-compose stop api

# API-ს გაშვება
docker-compose start api
```

### Logs-ის ნახვა

```powershell
# ყველა სერვისის logs
docker-compose logs -f

# მხოლოდ API logs
docker-compose logs -f api

# მხოლოდ PostgreSQL logs
docker-compose logs -f postgres

# ბოლო 100 ხაზი
docker-compose logs --tail=100 api
```

### Rebuild აპლიკაციის

```powershell
# კოდის შეცვლის შემდეგ
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained
docker-compose build --no-cache api
docker-compose up -d api
```

### Database Reset

```powershell
# 1. გამორთვა და ყველაფრის წაშლა
docker-compose down -v

# 2. PostgreSQL-ის გაშვება
docker-compose up -d postgres
Start-Sleep -Seconds 10

# 3. Migration თავიდან
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 4. API-ს გაშვება
docker-compose up -d
```

---

## 🐛 ხშირი პრობლემები

### ❌ "docker: command not found"

**მიზეზი:** Docker არ არის დაინსტალირებული ან არ არის PATH-ში

**გადაწყვეტა:**
1. დააინსტალირეთ Docker Desktop
2. გადაუტვირთეთ კომპიუტერი
3. გაუშვით Docker Desktop

---

### ❌ "Cannot connect to Docker daemon"

**მიზეზი:** Docker Desktop არ არის გაშვებული

**გადაწყვეტა:**
1. გახსენით Docker Desktop
2. დაელოდეთ სანამ სრულად ჩაიტვირთება
3. გაიმეორეთ ბრძანება

---

### ❌ Port 5432 already in use

**მიზეზი:** თქვენს კომპიუტერზე უკვე მუშაობს PostgreSQL

**გადაწყვეტა:**

**ვარიანტი 1:** გამორთეთ ლოკალური PostgreSQL
```powershell
# Windows Service-ის გამორთვა
Stop-Service postgresql-x64-16
```

**ვარიანტი 2:** შეცვალეთ პორტი `docker-compose.yml`-ში
```yaml
postgres:
  ports:
    - "5433:5432"  # შეცვალეთ 5432 → 5433
```

---

### ❌ Port 5000 already in use

**მიზეზი:** პორტი 5000 დაკავებულია

**გადაწყვეტა:**

**ვარიანტი 1:** მოკლეთ პროცესი
```powershell
# იპოვეთ პროცესი
netstat -ano | findstr :5000

# მოკლეთ (PID-ს ჩაანაცვლეთ)
taskkill /PID <PID> /F
```

**ვარიანტი 2:** შეცვალეთ პორტი `docker-compose.yml`-ში
```yaml
api:
  ports:
    - "5001:8080"  # გამოიყენეთ 5001
```

---

### ❌ "Unable to connect to database"

**შემოწმება:**
```powershell
# შეამოწმეთ PostgreSQL ჩაირთო თუ არა
docker-compose logs postgres

# შეამოწმეთ კონტეინერის სტატუსი
docker-compose ps
```

**გადაწყვეტა:**
1. დაელოდეთ 10-15 წამი PostgreSQL-ის სრულად ჩასატვირთად
2. შეამოწმეთ connection string
3. თუ არ მუშაობს, გადაიტვირთეთ PostgreSQL:
```powershell
docker-compose restart postgres
Start-Sleep -Seconds 10
```

---

### ❌ Migration-ის შეცდომები

**შეცდომა:** "A connection could not be made..."

**გადაწყვეტა:**
```powershell
# 1. შეამოწმეთ PostgreSQL
docker-compose ps postgres

# 2. დარწმუნდით რომ connection string სწორია
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"

# 3. თავიდან ცადეთ
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

---

### ❌ "No such file or directory: publish"

**მიზეზი:** არ გაკეთებულა `dotnet publish`

**გადაწყვეტა:**
```powershell
# Publish ხელახლა
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained

# შემდეგ build
docker-compose build --no-cache
```

---

### ❌ API იხსნება მაგრამ 404 ბრუნდება

**გადაწყვეტა:**
1. გახსენით: http://localhost:5000/swagger (არა https)
2. შეამოწმეთ logs:
```powershell
docker-compose logs -f api
```

---

## 🔄 Update Workflow

როდესაც კოდს ცვლით:

```powershell
# 1. Clean + Build
dotnet clean
dotnet build

# 2. Publish
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained

# 3. Rebuild Docker
docker-compose build --no-cache api

# 4. Restart
docker-compose up -d api

# 5. შეამოწმეთ logs
docker-compose logs -f api
```

---

## 📊 სასარგებლო ბრძანებები

```powershell
# Docker image-ების სია
docker images

# გაშვებული კონტეინერები
docker ps

# ყველა კონტეინერი (მათ შორის გაჩერებული)
docker ps -a

# Docker disk space
docker system df

# ძველი image-ების გასუფთავება
docker image prune -a

# ყველაფრის გასუფთავება (⚠️ ფრთხილად!)
docker system prune -a --volumes
```

---

## 🎯 Production Deployment

Production-ში გასატანად:

1. **შექმენით `.env` ფაილი:**
```env
POSTGRES_PASSWORD=strong_production_password
PGADMIN_PASSWORD=strong_admin_password
```

2. **განაახლეთ `docker-compose.yml`:**
```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
```

3. **Build და Deploy:**
```powershell
docker-compose -f docker-compose.yml up -d --build
```

---

## 📞 დახმარება

თუ პრობლემა გაქვთ:

1. ✅ შეამოწმეთ logs: `docker-compose logs -f`
2. ✅ შეამოწმეთ კონტეინერების სტატუსი: `docker-compose ps`
3. ✅ სცადეთ restart: `docker-compose restart`
4. ✅ სცადეთ rebuild: `docker-compose build --no-cache`
5. ✅ ბოლო საშუალება: `docker-compose down -v` და თავიდან setup

---

**წარმატებებს! 🚀**

