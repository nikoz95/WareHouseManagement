# 💻 ლოკალურად აპლიკაციის გაშვება (Docker-ის გარეშე)

## 📋 შინაარსი
- [წინაპირობები](#-წინაპირობები)
- [სწრაფი გაშვება](#-სწრაფი-გაშვება)
- [დეტალური ინსტრუქცია](#-დეტალური-ინსტრუქცია)
- [კონფიგურაცია](#-კონფიგურაცია)
- [Development Tools](#-development-tools)
- [ხშირი პრობლემები](#-ხშირი-პრობლემები)

---

## ✅ წინაპირობები

დარწმუნდით რომ დაინსტალირებულია:

### 1. .NET 9.0 SDK
```powershell
# ვერსიის შემოწმება
dotnet --version
# უნდა იყოს: 9.0.x
```

თუ არ არის დაინსტალირებული:
- [Download .NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### 2. PostgreSQL 16+
```powershell
# PostgreSQL-ის შემოწმება
psql --version
# უნდა იყოს: psql (PostgreSQL) 16.x
```

თუ არ არის დაინსტალირებული:
- [Download PostgreSQL](https://www.postgresql.org/download/)
- **ან** გამოიყენეთ Docker მხოლოდ PostgreSQL-ისთვის (რეკომენდებული)

### 3. IDE (Optional)
- Visual Studio 2022
- JetBrains Rider
- VS Code + C# Extension

---

## ⚡ სწრაფი გაშვება

### PowerShell Script (რეკომენდებული)

```powershell
.\start-local.ps1
```

### ხელით (Quick Version)

```powershell
# 1. Restore packages
dotnet restore

# 2. Database migration
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 3. Run
cd src/WareHouseManagement.API
dotnet run
```

---

## 📖 დეტალური ინსტრუქცია

### 1️⃣ PostgreSQL Setup

#### ვარიანტი A: Docker PostgreSQL (რეკომენდებული)

```powershell
# მხოლოდ PostgreSQL Docker-ში
docker-compose -f docker-compose.postgres.yml up -d

# ან
docker run -d `
  --name warehouse_postgres `
  -e POSTGRES_USER=warehouse_user `
  -e POSTGRES_PASSWORD=warehouse_pass_2024 `
  -e POSTGRES_DB=WareHouseManagementDb `
  -p 5432:5432 `
  postgres:16-alpine
```

#### ვარიანტი B: ლოკალური PostgreSQL

1. **დააინსტალირეთ PostgreSQL 16+**

2. **შექმენით ბაზა და user:**
```sql
-- PostgreSQL-ში შედით (psql)
CREATE USER warehouse_user WITH PASSWORD 'warehouse_pass_2024';
CREATE DATABASE WareHouseManagementDb OWNER warehouse_user;
GRANT ALL PRIVILEGES ON DATABASE WareHouseManagementDb TO warehouse_user;
```

3. **შეამოწმეთ კავშირი:**
```powershell
psql -h localhost -U warehouse_user -d WareHouseManagementDb
```

---

### 2️⃣ პროექტის Setup

#### ნაბიჯი 1: Clone/Download პროექტი

```powershell
cd C:\Projects
git clone <repository-url>
cd WareHouseManagment
```

#### ნაბიჯი 2: Restore NuGet Packages

```powershell
dotnet restore
```

ან Visual Studio/Rider-ში:
- File → Restore NuGet Packages

#### ნაბიჯი 3: კონფიგურაცია

**appsettings.Development.json** შეცვლა (თუ საჭიროა):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

---

### 3️⃣ Database Migration

```powershell
# Migration-ების გაშვება
dotnet ef database update `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API
```

ეს შექმნის:
- ✅ ყველა ცხრილს
- ✅ Relationships და Indexes
- ✅ Seed Data (demo მონაცემები)

**შემოწმება:**
```powershell
# PostgreSQL-ში შედით
psql -h localhost -U warehouse_user -d WareHouseManagementDb

# ცხრილების სია
\dt

# გასვლა
\q
```

---

### 4️⃣ აპლიკაციის გაშვება

#### ვარიანტი A: dotnet CLI

```powershell
cd src/WareHouseManagement.API
dotnet run
```

**ან Watch Mode (Auto-reload):**
```powershell
dotnet watch run
```

#### ვარიანტი B: Visual Studio

1. გახსენით `WareHouseManagement.sln`
2. Startup Project: `WareHouseManagement.API`
3. დააჭირეთ **F5** (Debug) ან **Ctrl+F5** (Run)

#### ვარიანტი C: JetBrains Rider

1. გახსენით `WareHouseManagement.sln`
2. Run Configuration: `WareHouseManagement.API`
3. დააჭირეთ **Shift+F10** (Run) ან **Shift+F9** (Debug)

---

### 5️⃣ შემოწმება

აპლიკაცია გაიშვა თუ ხედავთ:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**გახსენით ბრაუზერში:**
- **Swagger UI:** http://localhost:5000/swagger
- **API:** http://localhost:5000/api/companies

---

## 🔧 კონფიგურაცია

### Connection String ვარიანტები

#### Default (Development)
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
```

#### სხვა პორტი
```json
"DefaultConnection": "Host=localhost;Port=5433;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
```

#### Remote PostgreSQL
```json
"DefaultConnection": "Host=192.168.1.100;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024;SSL Mode=Require"
```

### Environment Variables

```powershell
# PowerShell-ში
$env:ASPNETCORE_ENVIRONMENT="Development"
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"

dotnet run
```

---

## 🛠️ Development Tools

### Entity Framework Core CLI

```powershell
# EF Tools-ის დაინსტალირება
dotnet tool install --global dotnet-ef

# ან განახლება
dotnet tool update --global dotnet-ef

# ვერსიის შემოწმება
dotnet ef --version
```

### Migration-ების მართვა

```powershell
# ახალი Migration-ის შექმნა
dotnet ef migrations add MigrationName `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API

# Migration-ის გაშვება
dotnet ef database update `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API

# Migration-ის გაუქმება (ბოლო migration-ის rollback)
dotnet ef database update PreviousMigrationName `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API

# ბაზის წაშლა
dotnet ef database drop `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API
```

### Database სკრიპტები

```powershell
# SQL Script-ის გენერაცია
dotnet ef migrations script `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API `
  --output migration.sql
```

---

## 🎯 Development Workflow

### 1. კოდის ცვლილება

```powershell
# Watch mode-ში გაშვება (auto-reload)
cd src/WareHouseManagement.API
dotnet watch run
```

ახლა კოდის ცვლილებისას ავტომატურად დარებილდება!

### 2. Database-ის ცვლილება

```powershell
# 1. Entity-ების შეცვლა Domain ან Infrastructure-ში
# 2. Migration-ის შექმნა
dotnet ef migrations add AddNewField `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API

# 3. Migration-ის გაშვება
dotnet ef database update `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API
```

### 3. Testing API

**Swagger UI-დან:**
1. http://localhost:5000/swagger
2. Try it out → Execute

**cURL-ით:**
```powershell
# GET request
curl http://localhost:5000/api/companies

# POST request
curl -X POST http://localhost:5000/api/companies `
  -H "Content-Type: application/json" `
  -d '{"name":"Test Company","taxId":"123456789","companyType":"Retail"}'
```

**PowerShell-ით:**
```powershell
# GET
Invoke-RestMethod -Uri "http://localhost:5000/api/companies"

# POST
$body = @{
    name = "Test Company"
    taxId = "123456789"
    companyType = "Retail"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/companies" `
  -Method Post `
  -Body $body `
  -ContentType "application/json"
```

---

## 🔍 Database Management Tools

### pgAdmin (გრაფიკული)

1. **დააინსტალირეთ:** [pgAdmin Download](https://www.pgadmin.org/download/)

2. **დაამატეთ Server:**
   - Host: `localhost`
   - Port: `5432`
   - Database: `WareHouseManagementDb`
   - Username: `warehouse_user`
   - Password: `warehouse_pass_2024`

### DBeaver (გრაფიკული)

1. **დააინსტალირეთ:** [DBeaver Download](https://dbeaver.io/download/)

2. **New Connection:** PostgreSQL
   - Host: `localhost`
   - Port: `5432`
   - Database: `WareHouseManagementDb`
   - Username: `warehouse_user`
   - Password: `warehouse_pass_2024`

### psql (Command Line)

```powershell
# კავშირი
psql -h localhost -U warehouse_user -d WareHouseManagementDb

# სასარგებლო ბრძანებები
\dt              # ცხრილების სია
\d table_name    # ცხრილის სტრუქტურა
\l               # ბაზების სია
\du              # users-ების სია
\q               # გასვლა

# SQL query-ები
SELECT * FROM "Companies";
SELECT * FROM "Products" LIMIT 10;
```

---

## 🐛 ხშირი პრობლემები

### ❌ "Unable to connect to database"

**შემოწმება:**
```powershell
# PostgreSQL-ის სტატუსი
pg_ctl status

# ან Windows Service
Get-Service postgresql*
```

**გადაწყვეტა:**
```powershell
# PostgreSQL-ის გაშვება (Windows Service)
Start-Service postgresql-x64-16

# ან Docker-ით
docker-compose -f docker-compose.postgres.yml up -d
```

---

### ❌ "dotnet: command not found"

**მიზეზი:** .NET SDK არ არის დაინსტალირებული ან არ არის PATH-ში

**გადაწყვეტა:**
1. დააინსტალირეთ [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
2. გადაუტვირთეთ PowerShell
3. შეამოწმეთ: `dotnet --version`

---

### ❌ "dotnet ef: command not found"

**მიზეზი:** EF Core Tools არ არის დაინსტალირებული

**გადაწყვეტა:**
```powershell
# დაინსტალირება
dotnet tool install --global dotnet-ef

# ან განახლება
dotnet tool update --global dotnet-ef
```

---

### ❌ Port 5000 already in use

**მიზეზი:** სხვა აპლიკაცია იყენებს პორტს 5000

**გადაწყვეტა:**

**ვარიანტი 1:** მოკლეთ პროცესი
```powershell
# იპოვეთ პროცესი
netstat -ano | findstr :5000

# მოკლეთ (PID ჩაანაცვლეთ)
taskkill /PID <PID> /F
```

**ვარიანტი 2:** შეცვალეთ პორტი
`appsettings.Development.json`:
```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5001"
      }
    }
  }
}
```

**ვარიანტი 3:** CLI-დან
```powershell
dotnet run --urls "http://localhost:5001"
```

---

### ❌ Migration-ის შეცდომები

**"No migrations found"**
```powershell
# შეამოწმეთ გზა
ls src/WareHouseManagement.Infrastructure/Migrations/

# თუ ცარიელია, შექმენით
dotnet ef migrations add InitialCreate `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API
```

**"Pending model changes"**
```powershell
# შექმენით ახალი migration
dotnet ef migrations add YourMigrationName `
  --project src/WareHouseManagement.Infrastructure `
  --startup-project src/WareHouseManagement.API
```

---

### ❌ NuGet Restore შეცდომები

**გადაწყვეტა:**
```powershell
# NuGet cache-ის გასუფთავება
dotnet nuget locals all --clear

# Restore თავიდან
dotnet restore

# ან Visual Studio/Rider-ში:
# Tools → NuGet Package Manager → Clear All NuGet Caches
```

---

### ❌ Build შეცდომები

```powershell
# Clean + Rebuild
dotnet clean
dotnet build

# ან კონკრეტული პროექტი
dotnet clean src/WareHouseManagement.API
dotnet build src/WareHouseManagement.API
```

---

## 📊 სასარგებლო ბრძანებები

### .NET CLI

```powershell
# პროექტების სია
dotnet sln list

# დამოკიდებულებები
dotnet list package

# დეტალური build
dotnet build --verbosity detailed

# Release build
dotnet build -c Release

# Tests (თუ არსებობს)
dotnet test
```

### Database

```powershell
# ბაზის სტრუქტურის export
pg_dump -h localhost -U warehouse_user -d WareHouseManagementDb -s > schema.sql

# ბაზის მონაცემების export
pg_dump -h localhost -U warehouse_user -d WareHouseManagementDb > backup.sql

# ბაზის restore
psql -h localhost -U warehouse_user -d WareHouseManagementDb < backup.sql
```

---

## 🚀 Performance Tips

### 1. Hot Reload გამოყენება

```powershell
# .NET 9.0 Hot Reload
dotnet watch run --no-hot-reload=false
```

### 2. Parallel Builds

```powershell
dotnet build --parallel
```

### 3. Development Database

პატარა development database-ისთვის:
```sql
-- PostgreSQL-ში არასაჭირო მონაცემების წაშლა
TRUNCATE TABLE "StockHistories" CASCADE;
TRUNCATE TABLE "Orders" CASCADE;
```

---

## 🎯 Production Build

```powershell
# Release build
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj `
  -c Release `
  -o publish `
  --no-self-contained

# გაშვება
cd publish
dotnet WareHouseManagement.API.dll
```

---

## 📞 დახმარება

თუ პრობლემა გაქვთ:

1. ✅ შეამოწმეთ PostgreSQL მუშაობს
2. ✅ შეამოწმეთ connection string სწორია
3. ✅ შეამოწმეთ .NET 9.0 დაინსტალირებულია
4. ✅ გაასუფთავეთ და თავიდან build: `dotnet clean && dotnet build`
5. ✅ თუ migration პრობლემაა, წაშალეთ ბაზა და თავიდან შექმენით

---

**წარმატებებს! 🚀**

