﻿# Warehouse Management System

Clean Architecture აპლიკაცია საწყობის მართვისთვის, აგებული .NET 9.0, PostgreSQL და Docker-ით.

## 📋 შინაარსი

- [სწრაფი დაწყება](#-სწრაფი-დაწყება)
- [API Endpoints](#-api-endpoints)
- [არქიტექტურა](#️-არქიტექტურა)
- [დეტალური გაიდები](#-დეტალური-გაიდები)

---

## 🚀 სწრაფი დაწყება

### 📦 ვარიანტი 1: Docker (რეკომენდებული)

**One-line setup:**
```powershell
.\start-docker.ps1
```

**ან ხელით:**
```powershell
docker-compose up -d postgres; Start-Sleep -Seconds 10; $env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"; dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API; docker-compose build api; docker-compose up -d
```

📖 **დეტალური ინსტრუქცია:** [DOCKER_QUICK_START.md](./DOCKER_QUICK_START.md)

---

### 💻 ვარიანტი 2: Local Development (Docker-ის გარეშე)

**წინაპირობები:** .NET 9.0 SDK + PostgreSQL 16+

```powershell
dotnet restore
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
cd src/WareHouseManagement.API
dotnet run
```

📖 **დეტალური ინსტრუქცია:** [LOCAL_DEVELOPMENT.md](./LOCAL_DEVELOPMENT.md)

---

## 🌐 ხელმისაწვდომი სერვისები

| სერვისი | URL | აღწერა |
|---------|-----|--------|
| **API Swagger** | http://localhost:5000/swagger | API დოკუმენტაცია და ტესტირება |
| **pgAdmin** | http://localhost:8080 | PostgreSQL UI (Docker only) |

---

## 🚀 სწრაფი დაწყება (Docker) - დეტალური

### წინაპირობები
- **Docker Desktop** - დაინსტალირებული და გაშვებული
- **.NET 9.0 SDK** - მიგრაციების გასაშვებად (optional)

### 🎯 რეკომენდებული: სრული Docker Setup

#### 1️⃣ პირველი გაშვება (ბაზის შექმნა)

```powershell
# 1. PostgreSQL-ის გაშვება
docker-compose up -d postgres

# დაელოდე 10 წამი PostgreSQL-ის ჩართვას
Start-Sleep -Seconds 10

# 2. მიგრაციების გაშვება (ცხრილების შექმნა + Seed Data)
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 3. API-ს და pgAdmin-ის გაშვება
docker-compose build api
docker-compose up -d
```

#### 2️⃣ შემდგომი გაშვებები

```powershell
# უბრალოდ გაუშვი ყველა სერვისი
docker-compose up -d
```

### 🌐 ხელმისაწვდომი სერვისები

| სერვისი | URL | მიღება |
|---------|-----|--------|
| **API (Swagger)** | http://localhost:5000/swagger | API დოკუმენტაცია და ტესტირება |
| **pgAdmin** | http://localhost:8080 | PostgreSQL მართვა |
| **PostgreSQL** | localhost:5432 | პირდაპირი წვდომა |

### 🔐 pgAdmin კონფიგურაცია

1. გახსენი http://localhost:8080
2. შედი:
   - **Email**: `admin@admin.com`
   - **Password**: `admin`

3. დაამატე სერვერი:
   - **General → Name**: `Warehouse Server`
   - **Connection**:
     - **Host**: `postgres` (Docker network-ში)
     - **Port**: `5432`
     - **Database**: `WareHouseManagementDb`
     - **Username**: `warehouse_user`
     - **Password**: `warehouse_pass_2024`

### 🛑 Docker-ის მართვა

```powershell
# გაჩერება
docker-compose stop

# გამორთვა
docker-compose down

# გამორთვა + ბაზის წაშლა (ყველაფრის წაშლა)
docker-compose down -v

# Logs-ის ნახვა
docker logs warehouse_api
docker logs warehouse_postgres

# კონკრეტული სერვისის რესტარტი
docker-compose restart api
```

## 📡 API Endpoints

ყველა endpoint იყენებს **lowercase kebab-case** სტილს და REST API კონვენციებს.

### Companies (კომპანიები)
```http
GET    /api/companies              # ყველა კომპანია
GET    /api/companies/{id}         # კონკრეტული კომპანია
POST   /api/companies              # ახალი კომპანიის შექმნა
PUT    /api/companies/{id}         # კომპანიის განახლება
DELETE /api/companies/{id}         # კომპანიის წაშლა (soft delete)
```

### Company Locations (კომპანიის ლოკაციები)
```http
GET    /api/companies/{companyId}/locations     # კომპანიის ყველა ლოკაცია
POST   /api/companies/{companyId}/locations     # ახალი ლოკაციის დამატება
```

### Products (პროდუქტები)
```http
GET    /api/products               # ყველა პროდუქტი
POST   /api/products               # ახალი პროდუქტის შექმნა
DELETE /api/products/{id}          # პროდუქტის წაშლა
```

### Warehouses (საწყობები)
```http
GET    /api/warehouses             # ყველა საწყობი
POST   /api/warehouses             # ახალი საწყობის შექმნა
DELETE /api/warehouses/{id}        # საწყობის წაშლა
```

### Warehouse Locations (საწყობის ლოკაციები)
```http
GET    /api/warehouses/{warehouseId}/locations  # საწყობის ყველა ლოკაცია
POST   /api/warehouses/{warehouseId}/locations  # ახალი ლოკაციის დამატება
```

### Warehouse Stocks (საწყობის მარაგი)
```http
GET    /api/warehouse-stocks       # ყველა მარაგი (ფილტრებით)
POST   /api/warehouse-stocks       # მარაგის დამატება

# Query Parameters:
# - warehouseLocationId
# - productId
# - manufacturerId
# - includePackagingDetails
# - includeAlcoholicDetails
```

### Stock History (მარაგის ისტორია)
```http
GET    /api/warehouse-stocks/history  # ტრანზაქციების ისტორია

# Query Parameters:
# - warehouseStockId
# - productId
# - orderId
# - transactionType
# - fromDate
# - toDate
```

### Orders (შეკვეთები)
```http
POST   /api/orders                 # ახალი შეკვეთის შექმნა
```

### Manufacturers (მწარმოებლები)
```http
GET    /api/manufacturers          # ყველა მწარმოებელი
POST   /api/manufacturers          # ახალი მწარმოებლის დამატება
DELETE /api/manufacturers/{id}     # მწარმოებლის წაშლა
```

### Unit Type Rules (საზომი ერთეულის წესები)
```http
GET    /api/unit-type-rules?onlyActive=true  # ყველა წესი
POST   /api/unit-type-rules                   # ახალი წესის შექმნა
DELETE /api/unit-type-rules/{id}              # წესის წაშლა
```

### Debtors (დებიტორები)
```http
GET    /api/debtors?companyId={id}  # დებიტორების სია
```

## 🏗️ არქიტექტურა

პროექტი აგებულია **Clean Architecture** პრინციპების მიხედვით:

### Layers:
- **Domain Layer** - ბიზნეს ლოგიკა და entity-ები
- **Application Layer** - CQRS (MediatR), DTOs, Validators, Mapping
- **Infrastructure Layer** - EF Core, PostgreSQL, Repositories
- **API Layer** - REST API Controllers, Swagger

### მთავარი ტექნოლოგიები:
- **.NET 9.0**
- **PostgreSQL** (Entity Framework Core)
- **MediatR** (CQRS Pattern)
- **AutoMapper**
- **FluentValidation**
- **Swagger/OpenAPI**

## 📋 ფუნქციონალი

### 1. კომპანიების მართვა (CRUD)
- პარტნიორი და არაპარტნიორი კომპანიების მართვა
- კომპანიის ტიპები: რესტორანი, ბარი, ქსელური კომპანია, რიტეილი
- ლოკაციების (ობიექტების) მართვა

### 2. პროდუქტების მართვა (CRUD)
- ალკოჰოლური პროდუქტების დამატება/რედაქტირება
- ბოთლების რაოდენობა და ყუთები (6 ბოთლი თითო ყუთში)
- ბარკოდი, მოცულობა, ალკოჰოლის პროცენტი

### 3. საწყობის მართვა
- საწყობის ლოკაციები და ობიექტები
- პროდუქტის რაოდენობა ცალობით და ყუთებად
- მწარმოებელი (ქარხანა) რომლიდანაც მარაგდება
- ვადის გასვლის თარიღი

### 4. შეკვეთების მართვა
- პარტნიორი და არაპარტნიორი კომპანიებისთვის
- შეკვეთის შექმნისას ავტომატურად:
  - ეკლება საწყობიდან რაოდენობა
  - ემატება დებიტორების სიაში
  - გამოითვლება ჯამური ღირებულება

### 5. დებიტორების სია
- ყველა დავალიანების ნახვა
- პარტნიორი/არაპარტნიორი დაჯგუფება
- გადახდების ისტორია

## 🚀 დაწყება

### წინაპირობები
- .NET 9.0 SDK
- PostgreSQL
- Visual Studio 2022 / JetBrains Rider

### ინსტალაცია

1. **Clone Repository**
```bash
git clone <repository-url>
cd WareHouseManagment
```

2. **Database Configuration**

`appsettings.json`-ში შეცვალეთ connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=WareHouseManagementDb;Username=your_username;Password=your_password"
  }
}
```

3. **Database Migration**

```bash
cd src/WareHouseManagement.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../WareHouseManagement.API
dotnet ef database update --startup-project ../WareHouseManagement.API
```

4. **Run Application**

```bash
cd src/WareHouseManagement.API
dotnet run
```

API ხელმისაწვდომია: `https://localhost:7xxx` ან `http://localhost:5xxx`

Swagger UI: `https://localhost:7xxx/swagger`

## 📚 API Endpoints

### Companies
- `GET /api/companies` - ყველა კომპანია
- `POST /api/companies` - კომპანიის შექმნა

### Products
- `GET /api/products` - ყველა პროდუქტი
- `POST /api/products` - პროდუქტის დამატება
- `PUT /api/products/{id}` - პროდუქტის რედაქტირება
- `DELETE /api/products/{id}` - პროდუქტის წაშლა

### Orders
- `POST /api/orders` - შეკვეთის შექმნა
- `GET /api/orders` - ყველა შეკვეთა
- `GET /api/orders/{id}` - შეკვეთის დეტალები

### Debtors
- `GET /api/debtors` - დებიტორების სია
- `GET /api/debtors/outstanding` - დავალიანებების სია

## 🗂️ პროექტის სტრუქტურა

```
WareHouseManagement/
├── src/
│   ├── WareHouseManagement.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── Interfaces/
│   │   └── Common/
│   ├── WareHouseManagement.Application/
│   │   ├── Features/
│   │   │   ├── Companies/
│   │   │   ├── Products/
│   │   │   └── Orders/
│   │   ├── DTOs/
│   │   ├── Mappings/
│   │   └── Validators/
│   ├── WareHouseManagement.Infrastructure/
│   │   ├── Data/
│   │   │   ├── Configurations/
│   │   │   └── ApplicationDbContext.cs
│   │   └── Repositories/
│   └── WareHouseManagement.API/
│       ├── Controllers/
│       └── Program.cs
```

## 🔑 მთავარი ფუნქციები

### შეკვეთის მაგალითი

```json
POST /api/orders
{
  "companyId": "guid-here",
  "customerName": "კომპანია XYZ",
  "customerPhone": "555123456",
  "orderItems": [
    {
      "productId": "product-guid",
      "quantityInBottles": 10,
      "quantityInBoxes": 2,
      "unitPrice": 15.50
    }
  ]
}
```

შეკვეთის შექმნისას:
1. ემატება შეკვეთა
2. ეკლება საწყობიდან
3. ემატება დებიტორების სიაში (თუ არ არის გადახდილი)

## 📝 შენიშვნები

- არაპარტნიორ კომპანიებზე რეპორტინგი შეიძლება იყოს შეზღუდული
- საწყობიდან პროდუქტი იკლება FIFO პრინციპით (ვადის მიხედვით)
- ყველა თარიღი ინახება UTC ფორმატში

## 🛠️ Development

### ახალი Migration-ის დამატება

```bash
cd src/WareHouseManagement.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../WareHouseManagement.API
dotnet ef database update --startup-project ../WareHouseManagement.API
```

### Build

```bash
dotnet build
```

### Tests (TODO)

```bash
dotnet test
```

---

## 📚 დეტალური გაიდები

### Setup & Deployment
- 📦 **[Docker Quick Start](./DOCKER_QUICK_START.md)** - სრული Docker setup ინსტრუქციები
- 💻 **[Local Development](./LOCAL_DEVELOPMENT.md)** - Docker-ის გარეშე development setup
- 🐛 **[Troubleshooting](./DOCKER_QUICK_START.md#-ხშირი-პრობლემები)** - ხშირი პრობლემები და გადაწყვეტები

### API & Testing
- 📡 **[API Testing Guide](./API_TESTING_GUIDE.md)** - API endpoints-ების ტესტირება
- 🔌 **[Swagger UI](http://localhost:5000/swagger)** - ინტერაქტიული API დოკუმენტაცია

### Scripts
- `start-docker.ps1` - ავტომატური Docker setup და გაშვება
- `start-quick.ps1` - სწრაფი local development გაშვება (PostgreSQL Docker-ში)

---

## 🎯 მთავარი Features

✅ **Clean Architecture** - Domain-Driven Design
✅ **CQRS Pattern** - MediatR-ით
✅ **Repository Pattern** - Unit of Work
✅ **FluentValidation** - Request validation
✅ **Entity Framework Core 9** - ORM
✅ **PostgreSQL** - Database
✅ **Mapperly** - Object mapping (Source Generator)
✅ **Docker Support** - Containerization
✅ **Swagger/OpenAPI** - API documentation
✅ **Seed Data** - Demo მონაცემებით

---

## 🛠️ Tech Stack

**Backend:**
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- PostgreSQL 16+

**Patterns & Libraries:**
- Clean Architecture
- CQRS (MediatR)
- Repository Pattern
- Unit of Work
- FluentValidation
- Mapperly (Source Generator)

**DevOps:**
- Docker & Docker Compose
- pgAdmin (Database Management)

---

## 👥 კონტრიბუცია

Contributions are welcome! Please feel free to submit a Pull Request.

---

## 📄 License

MIT License



