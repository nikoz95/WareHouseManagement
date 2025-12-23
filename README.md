# Warehouse Management System

Clean Architecture აპლიკაცია ალკოჰოლური პროდუქტების საწყობის მართვისთვის, აგებული .NET 9.0 და PostgreSQL-ით.

## 🚀 სწრაფი დაწყება (Docker)

### წინაპირობა
- Docker Desktop უნდა იყოს დაინსტალირებული და გაშვებული

### ვარიანტი 1: მარტივი გაშვება (რეკომენდებული)
```powershell
# გაუშვი სკრიპტი რომელიც PostgreSQL-ს Docker-ში ააქტიურებს და API-ს ლოკალურად
.\start-quick.ps1
```

### ვარიანტი 2: ყველაფერი Docker-ში
```powershell
# დააბილდე და გაუშვი PostgreSQL + API Docker-ში
docker-compose up --build

# ან background-ში გაშვება
docker-compose up -d --build
```

API ხელმისაწვდომი იქნება: http://localhost:5000
Swagger UI: http://localhost:5000/swagger

### ვარიანტი 3: მხოლოდ PostgreSQL Docker-ში
```powershell
# გაუშვი მხოლოდ PostgreSQL
docker-compose -f docker-compose.postgres.yml up -d

# API გაუშვი ლოკალურად
cd src\WareHouseManagement.API
dotnet ef database update
dotnet run
```

## 🛑 Docker-ის გამორთვა
```powershell
# გამორთე ყველა სერვისი
docker-compose down

# გამორთე და წაშალე ბაზის მონაცემები
docker-compose down -v
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

## 📄 License

MIT License

