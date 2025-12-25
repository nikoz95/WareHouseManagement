# 🚀 Warehouse Management System - Complete Setup Guide

ყოვლისმომცველი სახელმძღვანელო Warehouse Management System-ის დასაყენებლად და გასაშვებად.

---

## 📋 აირჩიე შენი გზა

### 🎯 რომელი ვარიანტი ავირჩიო?

| ვარიანტი | როდის გამოიყენო | სირთულე | სიჩქარე |
|----------|-----------------|----------|---------|
| **🐳 Docker** | Production, Team, CI/CD | ⭐⭐ | ⚡⚡⚡ |
| **💻 Local** | Solo Development, Learning | ⭐⭐⭐ | ⚡⚡ |

---

## 🐳 ვარიანტი 1: Docker Setup

### რატომ Docker?
✅ სწრაფი setup (5 წუთი)
✅ არ საჭიროებს PostgreSQL-ის დაინსტალირებას
✅ იდენტური გარემო production-ის
✅ pgAdmin ჩართული
✅ მარტივი გასუფთავება

### Quick Start

```powershell
# ერთი ბრძანება - ყველაფერი
.\start-docker.ps1
```

### რა მოხდება?
1. ✅ PostgreSQL კონტეინერი ჩაირთვება
2. ✅ Database შეიქმნება ავტომატურად
3. ✅ Migrations გადაიტარდება (ცხრილები + seed data)
4. ✅ API კონტეინერი აშენდება და ჩაირთვება
5. ✅ pgAdmin ჩაირთვება
6. ✅ Swagger გაიხსნება ბრაუზერში

### სერვისები
- 📡 API: http://localhost:5000/swagger
- 🗄️ pgAdmin: http://localhost:8080
- 💾 PostgreSQL: localhost:5432

### 📖 დეტალები
**სრული ინსტრუქცია:** [DOCKER_QUICK_START.md](./DOCKER_QUICK_START.md)

---

## 💻 ვარიანტი 2: Local Development

### რატომ Local?
✅ უფრო ლაბილი development
✅ სწრაფი build & reload
✅ უშუალო database access
✅ debugger ადვილად

### წინაპირობები

#### 1. .NET 9.0 SDK
```powershell
# შემოწმება
dotnet --version

# თუ არ გაქვს:
# https://dotnet.microsoft.com/download/dotnet/9.0
```

#### 2. PostgreSQL 16+
```powershell
# https://www.postgresql.org/download/windows/
# დაიმახსოვრე პაროლი რომელიც installation-ისას მიუთითე!
```

#### 3. pgAdmin (optional)
```powershell
# PostgreSQL-ს თან ერთვის
# ან https://www.pgadmin.org/download/
```

### Quick Start

```powershell
# ერთი ბრძანება - ყველაფერი
.\start-local.ps1
```

### ხელით Setup

```powershell
# 1. PostgreSQL-ის გაშვება
Start-Service postgresql-x64-16

# 2. პროექტის მომზადება
dotnet restore
dotnet build

# 3. Database setup
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 4. API-ს გაშვება
cd src/WareHouseManagement.API
dotnet run

# ან hot reload-ით:
dotnet watch run
```

### კონფიგურაცია

**appsettings.Development.json** - განაახლე connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

შეცვალე `YOUR_PASSWORD` შენი PostgreSQL პაროლით!

### სერვისები
- 📡 API: http://localhost:5000/swagger
- 🗄️ PostgreSQL: localhost:5432 (psql / pgAdmin)

### 📖 დეტალები
**სრული ინსტრუქცია:** [LOCAL_DEVELOPMENT.md](./LOCAL_DEVELOPMENT.md)

---

## 🎯 რა არის ნაგულისხმევად?

Setup-ის შემდეგ თქვენ გექნებათ:

### 📊 Demo Data (Seed Data)

#### კომპანიები
- 2 მომწოდებელი კომპანია (ღვინის მაღაზიები)
- 2 კლიენტი კომპანია (რესტორანი და ბარი)

#### მწარმოებლები
- 2 ღვინის/ალკოჰოლის მწარმოებელი

#### საწყობები
- 2 საწყობი
- თითოში 3 ლოკაცია (A-01, A-02, B-01)

#### პროდუქტები (10+)
- 🍷 **Saperavi 2020** - ქართული წითელი ღვინო
- 🍷 **Rkatsiteli 2021** - ქართული თეთრი ღვინო
- 🍾 **Georgian Sparkling** - ქართული იგრისტი
- 🥃 **Chacha Premium** - ქართული ჭაჭა
- 🍺 **Georgian Cider Apple** - ვაშლის სიდრი
- და სხვა...

#### შეკვეთები (20+)
- 10 დასრულებული შეკვეთა
- 10 მიმდინარე შეკვეთა
- თითოში 1-3 პროდუქტი

#### მარაგი
- ყველა პროდუქტი საწყობებშია განთავსებული
- ავტომატური packaging details
- ისტორია ყველა ტრანზაქციისა

### 🧪 გამოსაცდელად

1. **გახსენი Swagger:** http://localhost:5000/swagger
2. **შეეცადე:**
   - `GET /api/companies` - ნახე კომპანიები
   - `GET /api/products` - ნახე პროდუქტები
   - `GET /api/warehouse-stocks` - ნახე მარაგი
   - `POST /api/orders` - შექმენი შეკვეთა

3. **ნახე Database (pgAdmin):**
   - ყველა ცხრილი
   - მონაცემების სტრუქტურა
   - ურთიერთდამოკიდებულებები

---

## 📚 დოკუმენტაცია

### Setup Guides
- 🐳 [Docker Quick Start](./DOCKER_QUICK_START.md) - Docker setup (რეკომენდებული)
- 💻 [Local Development](./LOCAL_DEVELOPMENT.md) - Local setup
- 🐛 Troubleshooting - ხშირი პრობლემები

### API Reference
- 📡 [API Testing Guide](./API_TESTING_GUIDE.md) - Endpoints და Examples
- 🔌 [Swagger UI](http://localhost:5000/swagger) - ინტერაქტიული დოკუმენტაცია

### Scripts
- `start-docker.ps1` - Docker setup & start
- `start-local.ps1` - Local development start
- `start-quick.ps1` - Hybrid (PostgreSQL Docker, API local)

---

## 🔧 რას ვაკეთებ შემდეგ?

### Development Workflow

1. **გააგრძელე Development:**
   - ჩართე IDE (Rider, Visual Studio, VS Code)
   - გახსენი `WareHouseManagement.sln`
   - დაიწყე კოდის წერა

2. **API-ს ტესტირება:**
   - Swagger UI გამოიყენე: http://localhost:5000/swagger
   - ან Postman
   - ან curl/HTTP Client

3. **Database Changes:**
   ```powershell
   # ახალი migration
   dotnet ef migrations add YourMigrationName --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
   
   # apply
   dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
   ```

4. **ნახე Logs:**
   - Docker: `docker logs warehouse_api -f`
   - Local: Console-ში უბრალოდ ჩანს

---

## 🎓 რას სთავაზობს პროექტი?

### Architecture Patterns
- ✅ **Clean Architecture** - Domain-Driven Design
- ✅ **CQRS** - Command Query Responsibility Segregation
- ✅ **Repository Pattern** - Data Access Abstraction
- ✅ **Unit of Work** - Transaction Management

### Best Practices
- ✅ **Validation** - FluentValidation
- ✅ **Mapping** - Mapperly (Source Generator)
- ✅ **Logging** - Microsoft.Extensions.Logging
- ✅ **Error Handling** - Result Pattern
- ✅ **API Design** - RESTful, lowercase kebab-case

### Database
- ✅ **EF Core 9** - Code First
- ✅ **PostgreSQL** - Relational DB
- ✅ **Migrations** - Version Control for DB
- ✅ **Seed Data** - Demo მონაცემები

---

## 🆘 დახმარება

### ხშირი შეკითხვები

**Q: Docker container არ ეშვება?**
A: დარწმუნდი რომ Docker Desktop გაშვებულია და რომ პორტი 5432 თავისუფალია.

**Q: Migration ვერ გადაიტარა?**
A: შეამოწმე connection string appsettings.Development.json-ში.

**Q: როგორ დავარესეტო database?**
A: Docker: `docker-compose down -v && docker-compose up -d`
   Local: `dotnet ef database drop --force && dotnet ef database update`

**Q: Seed data არ ჩაიტვირთა?**
A: Seed data მხოლოდ ერთხელ ემატება. თუ გინდა თავიდან, წაშალე database და თავიდან შექმენი.

### Support

- 📖 სრული დოკუმენტაცია README-ში
- 🐛 პრობლემები? შეამოწმე Logs
- 💬 GitHub Issues

---

## 🎉 მზად ხარ!

აირჩიე შენი გზა და დაიწყე Development:

```powershell
# Docker
.\start-docker.ps1

# ან Local
.\start-local.ps1
```

**Happy Coding! 🚀**

