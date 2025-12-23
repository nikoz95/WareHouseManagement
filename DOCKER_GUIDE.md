# 🐳 Docker Setup Guide - WareHouse Management System

## წინაპირობები

### Docker Desktop-ის დაყენება
1. გადმოწერე Docker Desktop: https://www.docker.com/products/docker-desktop/
2. დააინსტალირე და გაუშვი
3. დარწმუნდი რომ Docker Desktop გაშვებულია (system tray-ში უნდა ჩანდეს Docker-ის აიქონი)

---

## 🚀 სწრაფი დაწყება - 3 ვარიანტი

### ✨ ვარიანტი 1: ავტომატური სკრიპტი (რეკომენდებული)

ყველაზე მარტივი გზა - სკრიპტი ყველაფერს გააკეთებს შენთვის:

```powershell
.\start-quick.ps1
```

**რას აკეთებს ეს სკრიპტი:**
1. ✅ ამოწმებს Docker-ის გაშვებულია თუ არა
2. ✅ PostgreSQL-ს Docker კონტეინერში ააქტიურებს
3. ✅ ელოდება PostgreSQL-ის სრულ ჩატვირთვას
4. ✅ ბაზის კავშირის სტრინგს განაახლებს
5. ✅ ბაზის მიგრაციებს გაუშვებს
6. ✅ შემოგთავაზებს API-ის გაშვებას

**შედეგი:**
- PostgreSQL მუშაობს Docker-ში (http://localhost:5432)
- ბაზა შექმნილია და მიგრირებულია
- API გაშვებული (თუ დაეთანხმე)

---

### 🐋 ვარიანტი 2: სრული Docker Compose Setup

PostgreSQL + API ორივე Docker-ში:

```powershell
# დააბილდე და გაუშვი ყველაფერი
docker-compose up --build

# ან background-ში (detached mode)
docker-compose up -d --build
```

**ხელმისაწვდომი სერვისები:**
- 🌐 API: http://localhost:5000
- 📚 Swagger UI: http://localhost:5000/swagger
- 🐘 PostgreSQL: localhost:5432

**ლოგების ნახვა:**
```powershell
# ყველა სერვისის ლოგები
docker-compose logs -f

# მხოლოდ API
docker-compose logs -f api

# მხოლოდ PostgreSQL
docker-compose logs -f postgres
```

**გამორთვა:**
```powershell
# სერვისების გამორთვა
docker-compose down

# სერვისები + ბაზის მონაცემები
docker-compose down -v
```

---

### 🔧 ვარიანტი 3: მხოლოდ PostgreSQL Docker-ში, API ლოკალურად

თუ API-ს ლოკალურად უნდა გაუშვა debugging-ისთვის:

```powershell
# 1. PostgreSQL-ის გაშვება Docker-ში
docker-compose -f docker-compose.postgres.yml up -d

# 2. ელოდება PostgreSQL-ის ready status-ს
Start-Sleep -Seconds 10

# 3. მიგრაციების გაშვება
cd src\WareHouseManagement.API
dotnet ef database update

# 4. API-ის გაშვება
dotnet run
```

**API ხელმისაწვდომი:** http://localhost:5000 (ან რაც Program.cs-ში იქნება)

---

## 📊 ბაზის კავშირის ინფორმაცია

### Docker PostgreSQL Credentials:
```
Host:     localhost
Port:     5432
Database: WareHouseManagementDb
Username: warehouse_user
Password: warehouse_pass_2024
```

### Connection String:
```
Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024
```

---

## 🔧 სასარგებლო ბრძანებები

### Docker Compose
```powershell
# ყველა სერვისის გაშვება
docker-compose up

# Background-ში გაშვება
docker-compose up -d

# Rebuild და გაშვება
docker-compose up --build

# გამორთვა
docker-compose down

# გამორთვა + volumes წაშლა (ბაზის მონაცემები)
docker-compose down -v

# სერვისების სტატუსი
docker-compose ps

# ლოგები
docker-compose logs -f
```

### PostgreSQL-ში შესვლა
```powershell
# PostgreSQL-ის კონტეინერში შესვლა
docker exec -it warehouse_postgres psql -U warehouse_user -d WareHouseManagementDb

# ან
docker-compose exec postgres psql -U warehouse_user -d WareHouseManagementDb
```

### ბაზის მიგრაციები
```powershell
# ლოკალურად
cd src\WareHouseManagement.API
dotnet ef migrations add MigrationName
dotnet ef database update

# Docker კონტეინერში
docker-compose exec api dotnet ef database update
```

---

## 🐛 Troubleshooting

### Docker Desktop არ გაშვებულა
```
Error: error during connect...
```
**გადაწყვეტა:** გაუშვი Docker Desktop და დაელოდე სანამ სრულად ჩაიტვირთება.

### Port-ი უკვე დაკავებულია
```
Error: port is already allocated
```
**გადაწყვეტა:** შეცვალე port-ები `docker-compose.yml`-ში:
```yaml
ports:
  - "5433:5432"  # PostgreSQL
  - "5001:8080"  # API
```

### PostgreSQL არ უკავშირდება
```
Failed to connect to 127.0.0.1:5432
```
**გადაწყვეტა:**
1. დარწმუნდი რომ PostgreSQL კონტეინერი გაშვებულია: `docker ps`
2. დაელოდე რამდენიმე წამი healthcheck-ს
3. შეამოწმე ლოგები: `docker-compose logs postgres`

### მიგრაციები არ გადის
```
An error occurred using the connection to database...
```
**გადაწყვეტა:**
1. დარწმუნდი რომ PostgreSQL ready-ა
2. შეამოწმე connection string `appsettings.Development.json`-ში
3. მანუალურად გაუშვი: `dotnet ef database update`

### ბაზის reset-ი სჭირდება
```powershell
# გამორთე და წაშალე ბაზა
docker-compose down -v

# თავიდან გაუშვი
docker-compose up -d
cd src\WareHouseManagement.API
dotnet ef database update
```

---

## 📚 დამატებითი რესურსები

- **Docker SETUP**: `DOCKER_SETUP.md`
- **პროექტის დოკუმენტაცია**: `README.md`
- **Setup Instructions**: `SETUP_INSTRUCTIONS.md`

---

## ✅ Success Checklist

- [ ] Docker Desktop დაინსტალირებული და გაშვებული
- [ ] `start-quick.ps1` წარმატებით გაეშვა
- [ ] PostgreSQL container გაშვებულია: `docker ps | findstr postgres`
- [ ] ბაზა შექმნილია და მიგრირებულია
- [ ] API პასუხობს: http://localhost:5000/swagger
- [ ] Swagger UI იხსნება და API endpoints-ები ჩანს

თუ ყველა checkbox ✅, მაშინ ყველაფერი მუშაობს! 🎉

