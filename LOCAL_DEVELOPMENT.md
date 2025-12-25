# 🖥️ Local Development Setup (Without Docker)

Warehouse Management System-ის გაშვება ლოკალურად Docker-ის გარეშე.

## 📋 წინაპირობები

### 1. .NET 9.0 SDK
```powershell
# შემოწმება
dotnet --version

# თუ არ გაქვს, ჩამოტვირთე:
# https://dotnet.microsoft.com/download/dotnet/9.0
```

### 2. PostgreSQL
```powershell
# ჩამოტვირთე და დააინსტალირე:
# https://www.postgresql.org/download/windows/

# რეკომენდებული: PostgreSQL 16 ან უფრო ახალი
```

### 3. IDE (არჩევანი)
- **JetBrains Rider** (რეკომენდებული)
- **Visual Studio 2022**
- **Visual Studio Code** + C# Extension

---

## 🗄️ PostgreSQL Setup

### ნაბიჯი 1: PostgreSQL Service-ის გაშვება

```powershell
# Windows Services-იდან გაუშვი PostgreSQL
# ან
Start-Service postgresql-x64-16  # შეცვალე 16 თქვენი ვერსიით
```

### ნაბიჯი 2: Database-ის შექმნა

#### Option A: pgAdmin-ით (GUI)

1. გახსენი pgAdmin
2. დაუკავშირდი PostgreSQL სერვერს
3. Databases → Create → Database
   - **Database name**: `WareHouseManagementDb`
   - **Owner**: `postgres`
4. Save

#### Option B: psql-ით (Terminal)

```powershell
# psql-ში შესვლა
psql -U postgres

# Database-ის შექმნა
CREATE DATABASE "WareHouseManagementDb";

# User-ის შექმნა (optional, უსაფრთხოებისთვის)
CREATE USER warehouse_user WITH PASSWORD 'warehouse_pass_2024';
GRANT ALL PRIVILEGES ON DATABASE "WareHouseManagementDb" TO warehouse_user;

# გასვლა
\q
```

---

## ⚙️ პროექტის კონფიგურაცია

### ნაბიჯი 1: Connection String-ის განახლება

#### **appsettings.Development.json** (რეკომენდებული)

გახსენი:
```
src/WareHouseManagement.API/appsettings.Development.json
```

განაახლე:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=postgres;Password=YOUR_POSTGRES_PASSWORD"
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

⚠️ **მნიშვნელოვანი**: შეცვალე `YOUR_POSTGRES_PASSWORD` თქვენი PostgreSQL პაროლით!

#### ან User Secrets-ით (უსაფრთხო მეთოდი)

```powershell
cd src/WareHouseManagement.API

# User secrets-ის ინიციალიზაცია
dotnet user-secrets init

# Connection string-ის დამატება
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=postgres;Password=YOUR_PASSWORD"
```

---

## 🚀 პროექტის გაშვება

### მეთოდი 1: Terminal-იდან

#### ნაბიჯი 1: დეპენდენციების აღდგენა
```powershell
cd C:\Users\Nmalidze\RiderProjects\WareHouseManagment

# Restore packages
dotnet restore
```

#### ნაბიჯი 2: Build
```powershell
dotnet build
```

#### ნაბიჯი 3: Database Migrations
```powershell
# მიგრაციების გაშვება (ცხრილების შექმნა + Seed Data)
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

**რას აკეთებს:**
- ✅ ქმნის ყველა ცხრილს
- ✅ ამატებს საწყის მონაცემებს:
  - 4 კომპანია
  - 2 მწარმოებელი
  - 2 საწყობი
  - 10+ პროდუქტი
  - 20+ შეკვეთა

#### ნაბიჯი 4: API-ს გაშვება
```powershell
cd src/WareHouseManagement.API
dotnet run
```

ან watch mode-ში (auto-reload):
```powershell
dotnet watch run
```

#### ✅ მზადაა!
```
API:     http://localhost:5000
Swagger: http://localhost:5000/swagger
```

---

### მეთოდი 2: Visual Studio / Rider-იდან

#### Visual Studio 2022:

1. გახსენი `WareHouseManagement.sln`
2. **Solution Explorer** → **WareHouseManagement.API** → Set as Startup Project
3. **Tools** → **NuGet Package Manager** → **Package Manager Console**:
   ```powershell
   Update-Database
   ```
4. დააჭირე **F5** (Run) ან **Ctrl+F5** (Run without debugging)

#### JetBrains Rider:

1. გახსენი `WareHouseManagement.sln`
2. **Terminal** (Alt+F12):
   ```powershell
   dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
   ```
3. **Run** → **Run 'WareHouseManagement.API'** ან დააჭირე **Shift+F10**

---

## 🔧 სასარგებლო ბრძანებები

### Database Migrations

#### ახალი მიგრაციის შექმნა
```powershell
dotnet ef migrations add MigrationName --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

#### მიგრაციების სია
```powershell
dotnet ef migrations list --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

#### Database-ის განახლება
```powershell
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

#### კონკრეტულ მიგრაციაზე დაბრუნება
```powershell
dotnet ef database update MigrationName --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

#### ბოლო მიგრაციის გაუქმება
```powershell
dotnet ef migrations remove --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

#### Database-ის წაშლა (ყველაფრის თავიდან)
```powershell
dotnet ef database drop --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

---

### Build & Run Commands

#### Clean Build
```powershell
dotnet clean
dotnet build
```

#### Release Build
```powershell
dotnet build -c Release
```

#### Publish (deployment-ისთვის)
```powershell
dotnet publish -c Release -o ./publish
```

#### Run specific project
```powershell
dotnet run --project src/WareHouseManagement.API
```

#### Run with environment variable
```powershell
$env:ASPNETCORE_ENVIRONMENT="Production"
dotnet run --project src/WareHouseManagement.API
```

---

### Testing

#### Run all tests
```powershell
dotnet test
```

#### Run with coverage
```powershell
dotnet test /p:CollectCoverage=true
```

---

## 📊 Database Management

### pgAdmin-ით მუშაობა

1. გახსენი **pgAdmin**
2. **Servers** → **PostgreSQL 16** → **Databases** → **WareHouseManagementDb**
3. შეგიძლია:
   - ნახო ცხრილების სტრუქტურა
   - გაუშვა SQL queries
   - დაათვალიერო მონაცემები

### SQL Client-ით (psql)

```powershell
# Database-ში შესვლა
psql -U postgres -d WareHouseManagementDb

# სასარგებლო ბრძანებები:
\dt                          # ცხრილების სია
\d+ "TableName"              # ცხრილის სტრუქტურა
SELECT * FROM "Companies";   # მონაცემების ნახვა
\q                           # გასვლა
```

---

## 🐛 ხშირი პრობლემები და გადაწყვეტები

### ❌ "Cannot connect to database"

**შემოწმება:**
```powershell
# PostgreSQL service მუშაობს?
Get-Service postgresql-x64-*

# თუ გამორთულია, ჩართე:
Start-Service postgresql-x64-16
```

**Connection String შეამოწმე:**
- Host სწორია? (`localhost`)
- Port სწორია? (`5432`)
- პაროლი სწორია?

**Test Connection:**
```powershell
psql -U postgres -d WareHouseManagementDb
# თუ ჩაუვლი, connection string-იც სწორია
```

---

### ❌ "Build failed" - შეცდო���ები

**NuGet Packages-ის აღდგენა:**
```powershell
dotnet restore
dotnet clean
dotnet build
```

**NuGet Cache-��ს გასუფთავება:**
```powershell
dotnet nuget locals all --clear
dotnet restore
```

---

### ❌ "Migration failed"

**Database-ის თავიდან შექმნა:**
```powershell
# 1. Database წაშლა
dotnet ef database drop --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API --force

# 2. თავიდან შექმნა
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

---

### ❌ "Port 5000 already in use"

**პორტის შეცვლა:**

`src/WareHouseManagement.API/Properties/launchSettings.json`:
```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:5001"  // შეცვალე 5001-ზე
    }
  }
}
```

ან Environment Variable-ით:
```powershell
$env:ASPNETCORE_URLS="http://localhost:5001"
dotnet run --project src/WareHouseManagement.API
```

---

### ❌ "Seed data არ ემატება"

**მიზეზი:** უკვე არსებობს მონაცემები

**გადაწყვეტა:**
```powershell
# Database-ის თავიდან შექმნა
dotnet ef database drop --force --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

---

## 🔐 Production Setup

### Environment Variables

Production-ზე არ გამოიყენო `appsettings.json` პაროლებისთვის!

#### Windows:
```powershell
# System Environment Variables
[Environment]::SetEnvironmentVariable("ConnectionStrings__DefaultConnection", "Host=prod-server;...", "Machine")
```

#### Linux/Docker:
```bash
export ConnectionStrings__DefaultConnection="Host=prod-server;..."
```

---

## 📁 პროექტის სტრუქტურა

```
WareHouseManagement/
├── src/
│   ├── WareHouseManagement.API/          # Web API
│   │   ├── Controllers/                  # API Endpoints
│   │   ├── appsettings.json             # კონფიგურაცია
│   │   └── Program.cs                    # Entry Point
│   ├── WareHouseManagement.Application/  # Business Logic
│   │   ├── Features/                     # CQRS Commands/Queries
│   │   ├── DTOs/                         # Data Transfer Objects
│   │   └── Validators/                   # FluentValidation
│   ├── WareHouseManagement.Infrastructure/ # Data Access
│   │   ├── Data/                         # EF Core Context
│   │   ├── Migrations/                   # Database Migrations
│   │   └── Repositories/                 # Repository Pattern
│   └── WareHouseManagement.Domain/       # Domain Models
│       ├── Entities/                     # Domain Entities
│       └── Enums/                        # Enumerations
└── WareHouseManagement.sln               # Solution File
```

---

## ⚡ Quick Start Script

შექმენი `start-local.ps1`:

```powershell
# Start Local Development
Write-Host "🚀 Starting Warehouse Management System (Local)" -ForegroundColor Cyan

# 1. Check PostgreSQL
Write-Host "Checking PostgreSQL..." -ForegroundColor Yellow
$pg = Get-Service postgresql-x64-* -ErrorAction SilentlyContinue
if ($pg.Status -ne "Running") {
    Write-Host "Starting PostgreSQL..." -ForegroundColor Yellow
    Start-Service $pg.Name
    Start-Sleep -Seconds 3
}

# 2. Restore & Build
Write-Host "Building project..." -ForegroundColor Yellow
dotnet restore
dotnet build

# 3. Run Migrations
Write-Host "Running migrations..." -ForegroundColor Yellow
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 4. Run API
Write-Host "Starting API..." -ForegroundColor Yellow
Write-Host ""
Write-Host "✅ Ready!" -ForegroundColor Green
Write-Host "API: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host ""

cd src/WareHouseManagement.API
dotnet watch run
```

შემდეგ გაუშვი:
```powershell
.\start-local.ps1
```

---

## 🎯 Development Workflow

1. **PostgreSQL-ის გაშვება** - Start-Service
2. **კოდის ცვლილება** - შენი IDE-ით
3. **Migrations** - თუ Schema შეიცვალა:
   ```powershell
   dotnet ef migrations add MyChange --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
   dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
   ```
4. **API გაშვება** - `dotnet watch run`
5. **ტესტირება** - Swagger-ით http://localhost:5000/swagger

---

## 📞 დახმარება

**Logs:**
```powershell
# Console-ში უბრალოდ ჩანს ყველაფერი
# თუ გინდა ფაილში:
dotnet run --project src/WareHouseManagement.API > api.log 2>&1
```

**Debug Mode:**
```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
$env:Logging__LogLevel__Default="Debug"
dotnet run --project src/WareHouseManagement.API
```

---

**მზადაა! 🎉 შეგიძლია დაიწყო Development!**

