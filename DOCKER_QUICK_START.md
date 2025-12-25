# 🐳 Docker Quick Start Guide

სწრაფი დაწყების ინსტრუქცია Warehouse Management System-ისთვის Docker-ით.

## 📋 წინაპირობები

1. **Docker Desktop** - [ჩამოტვირთე აქ](https://www.docker.com/products/docker-desktop)
2. **.NET 9.0 SDK** (optional, მიგრაციებისთვის) - [ჩამოტვირთე აქ](https://dotnet.microsoft.com/download)
3. **PowerShell** - Windows-ზე უკვე დაინსტალირებული

## 🚀 პირველი გაშვება (Step by Step)

### ნაბიჯი 1: Docker Desktop-ის გაშვება

```powershell
# დარწმუნდი რომ Docker Desktop გაშვებულია
docker --version
docker-compose --version
```

### ნაბიჯი 2: პროექტის დირექტორიაში გადასვლა

```powershell
cd C:\Users\Nmalidze\RiderProjects\WareHouseManagment
```

### ნაბიჯი 3: PostgreSQL-ის გაშვება

```powershell
# გაუშვი მხოლოდ PostgreSQL კონტეინერი
docker-compose up -d postgres

# დაელოდე PostgreSQL-ის სრულად ჩართვას (10 წამი)
Start-Sleep -Seconds 10
```

### ნაბიჯი 4: მიგრაციების გაშვება (ბაზის შექმნა)

```powershell
# დააყენე connection string
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"

# გაუშვი მიგრაციები (ცხრილების შექმნა + seed data)
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

**რას აკეთებს ეს ბრძანება:**
- ქმნის ყველა საჭირო ცხრილს PostgreSQL-ში
- ამატებს საწყის მონაცემებს (seed data):
  - 2 კომპანია (მომწოდებლები)
  - 2 კომპანია (კლიენტები)
  - 2 მწარმოებელი
  - 2 საწყობი
  - 10+ პროდუქტი (ალკოჰოლური სასმელები + სიდრი)
  - 20+ შეკვეთა (მიმდინარე და დასრულებული)

### ნაბიჯი 5: API-ს და pgAdmin-ის გაშვება

```powershell
# აიბილდე API Docker image
docker-compose build api

# გაუშვი ყველა სერვისი background-ში
docker-compose up -d
```

### ნაბიჯი 6: შემოწმება

```powershell
# შეამოწმე კონტეინერების სტატუსი
docker-compose ps

# უნდა ნახო:
# warehouse_postgres  - healthy/running
# warehouse_api       - running
# warehouse_pgadmin   - running
```

### ნაბიჯი 7: გახსენი ბრაუზერში

```powershell
# გახსენი Swagger UI
Start-Process "http://localhost:5000/swagger"

# გახსენი pgAdmin (optional)
Start-Process "http://localhost:8080"
```

## 🎉 მზადაა! 

API ხელმისაწვდომია: **http://localhost:5000/swagger**

---

## 🔄 შემდგომი გაშვებები

თუ უკვე გაიარე ზემოთ ნაბიჯები, შემდგომში მარტივად:

```powershell
# უბრალოდ გაუშვი
docker-compose up -d

# ან თუ კოდი შეცვალე
docker-compose build api
docker-compose up -d
```

---

## 🔧 სასარგებლო ბრძანებები

### Logs-ის ნახვა

```powershell
# API logs
docker logs warehouse_api

# PostgreSQL logs
docker logs warehouse_postgres

# ყველა logs real-time
docker-compose logs -f
```

### კონტეინერების მართვა

```powershell
# გაჩერება (მონაცემები ინახება)
docker-compose stop

# გაშვება ხელახლა
docker-compose start

# რესტარტი
docker-compose restart

# სრული გამორთვა
docker-compose down

# გამორთვა + ბაზის წაშლა
docker-compose down -v
```

### ბაზაში წვდომა

```powershell
# psql-ით შესვლა
docker exec -it warehouse_postgres psql -U warehouse_user -d WareHouseManagementDb

# შიგნიდან SQL ბრძანებები:
# \dt              - ცხრილების სია
# \d+ table_name   - ცხრილის სტრუქტურა
# SELECT * FROM "Companies";
# \q               - გასვლა
```

### მიგრაციების მართვა

```powershell
# ახალი მიგრაციის შექმნა
dotnet ef migrations add MigrationName --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# მიგრაციების სია
dotnet ef migrations list --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# ბაზის განახლება
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

---

## 🐛 ხშირი პრობლემები

### ❌ "Cannot connect to Docker daemon"

**მიზეზი:** Docker Desktop არ არის გაშვებული

**გადაწყვეტა:**
1. გაუშვი Docker Desktop
2. დაელოდე სრულად ჩართვას (whale icon-ი უნდა იყოს მწვანე)
3. სცადე ხელახლა

### ❌ "Port 5432 is already allocated"

**მიზეზი:** უკვე გაშვებულია PostgreSQL თქვენს სისტემაში

**გადაწყვეტა 1 (რეკომენდებული):**
```powershell
# გამორთე ლოკალური PostgreSQL სერვისი
Stop-Service postgresql-x64-*
```

**გადაწყვეტა 2:**
```powershell
# შეცვალე პორტი docker-compose.yml-ში
# ports:
#   - "5433:5432"  # ახალი პორტი
```

### ❌ "relation 'Companies' does not exist"

**მიზეზი:** მიგრაციები არ გადატარებულა

**გადაწყვეტა:**
```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

### ❌ API არ ხსნის Swagger-ს

**შემოწმება:**
```powershell
# API logs
docker logs warehouse_api

# კონტეინერის სტატუსი
docker-compose ps
```

**გადაწყვეტა:**
```powershell
# რესტარტი
docker-compose restart api

# თუ არ მუშაობს, თავიდან აიბილდე
docker-compose build api
docker-compose up -d api
```

---

## 🔐 Credentials

### PostgreSQL
- **Host:** localhost
- **Port:** 5432
- **Database:** WareHouseManagementDb
- **Username:** warehouse_user
- **Password:** warehouse_pass_2024

### pgAdmin
- **URL:** http://localhost:8080
- **Email:** admin@admin.com
- **Password:** admin

---

## 📊 pgAdmin Setup

1. გახსენი http://localhost:8080
2. შედი credentials-ით
3. **Add New Server**:
   - **General**:
     - Name: `Warehouse Server`
   - **Connection**:
     - Host: `postgres` (არა localhost!)
     - Port: `5432`
     - Maintenance database: `WareHouseManagementDb`
     - Username: `warehouse_user`
     - Password: `warehouse_pass_2024`
     - Save password: ✓
4. **Save**

ახლა შეგიძლია ნახო ყველა ცხრილი, მონაცემები და გაუშვა SQL queries.

---

## 🧹 სრული დასუფთავება

თუ გინდა ყველაფრის თავიდან დაწყება:

```powershell
# გამორთე ყველაფერი და წაშალე volumes
docker-compose down -v

# წაშალე images
docker rmi warehousemanagment-api

# თავიდან დაწყება
docker-compose up -d postgres
# ... დაელოდე და გაუშვი მიგრაციები
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
docker-compose build api
docker-compose up -d
```

---

## 📞 დახმარება

თუ პრობლემა გაქვს:

1. შეამოწმე logs: `docker-compose logs`
2. შეამოწმე კონტეინერების სტატუსი: `docker-compose ps`
3. დარწმუნდი რომ Docker Desktop გაშვებულია
4. სცადე რესტარტი: `docker-compose restart`

---

## ⚡ One-Line სრული Setup

თუ გინდა ყველაფერი ერთი ბრძანებით:

```powershell
docker-compose up -d postgres; Start-Sleep -Seconds 10; $env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"; dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API; docker-compose build api; docker-compose up -d; Start-Sleep -Seconds 5; Start-Process "http://localhost:5000/swagger"
```

**რას აკეთებს:**
1. ხსნის PostgreSQL
2. ელოდება 10 წამი
3. გაუშვება მიგრაციებს
4. აიბილდებს API
5. გაუშვება ყველაფერს
6. გახსნის Swagger-ს ბრაუზერში

---

**მზადაა! 🎉 დაიწყე API-ს ტესტირება Swagger-ით!**

