# ⚡ სწრაფი სახელმძღვანელო - Warehouse Management System

## 🎯 რას ვირჩევ?

```
Docker-ით გაშვება (რეკომენდებული) ⟶ DOCKER_START_GUIDE.md
ლოკალურად გაშვება            ⟶ LOCAL_START_GUIDE.md
```

---

## 🐳 Docker-ით (სწრაფი)

### ერთი ბრძანება
```powershell
.\start-docker.ps1
```

### ხელით
```powershell
# 1. PostgreSQL
docker-compose up -d postgres
Start-Sleep -Seconds 10

# 2. Migration
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024"
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 3. Build & Run
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained
docker-compose build --no-cache
docker-compose up -d
```

**ბრაუზერში:** http://localhost:5000/swagger

📖 [დეტალური ინსტრუქცია](./DOCKER_START_GUIDE.md)

---

## 💻 ლოკალურად (სწრაფი)

### ერთი ბრძანება
```powershell
.\start-local.ps1
```

### ხელით
```powershell
# 1. PostgreSQL (Docker-ით)
docker-compose -f docker-compose.postgres.yml up -d
Start-Sleep -Seconds 10

# 2. Migration
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# 3. Run
cd src/WareHouseManagement.API
dotnet run
```

**ბრაუზერში:** http://localhost:5000/swagger

📖 [დეტალური ინსტრუქცია](./LOCAL_START_GUIDE.md)

---

## 🛠️ სასარგებლო ბრძანებები

### Docker
```powershell
docker-compose ps              # სტატუსი
docker-compose logs -f api     # logs
docker-compose restart api     # restart
docker-compose down            # გამორთვა
docker-compose down -v         # გამორთვა + ბაზის წაშლა
```

### Migration
```powershell
# ახალი Migration
dotnet ef migrations add MigrationName --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API

# Migration-ის გაშვება
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

### Build & Run
```powershell
dotnet clean              # გასუფთავება
dotnet build              # build
dotnet watch run          # run with auto-reload
```

---

## 🌐 URLs

| სერვისი | URL |
|---------|-----|
| **API (Swagger)** | http://localhost:5000/swagger |
| **pgAdmin** | http://localhost:8080 |
| **PostgreSQL** | localhost:5432 |

---

## 🔐 Credentials

### PostgreSQL
```
Host: localhost
Port: 5432
Database: WareHouseManagementDb
Username: warehouse_user
Password: warehouse_pass_2024
```

### pgAdmin
```
Email: admin@admin.com
Password: admin
```

---

## 🔄 კოდის შეცვლის შემდეგ

### Docker-ით
```powershell
dotnet publish src/WareHouseManagement.API/WareHouseManagement.API.csproj -c Release -o publish --no-self-contained
docker-compose build --no-cache api
docker-compose up -d api
```

### ლოკალურად
```powershell
# უბრალოდ Ctrl+C და თავიდან
dotnet run

# ან watch mode-ში ავტომატურად განახლდება
dotnet watch run
```

---

## 🐛 რა ვქნა თუ...

### ❌ Docker არ მუშაობს
1. გაუშვით Docker Desktop
2. სცადეთ: `docker --version`
3. თუ მაინც არ მუშაობს → [Docker გაიდი](./DOCKER_START_GUIDE.md#-ხშირი-პრობლემები)

### ❌ Port 5000 დაკავებულია
```powershell
# იპოვეთ პროცესი
netstat -ano | findstr :5000

# მოკლეთ
taskkill /PID <PID> /F
```

### ❌ Database-თან კავშირი არ არის
```powershell
# შეამოწმეთ PostgreSQL
docker-compose ps postgres

# Restart
docker-compose restart postgres
Start-Sleep -Seconds 10
```

### ❌ Migration-ის პრობლემა
```powershell
# ბაზის წაშლა და თავიდან
docker-compose down -v
docker-compose up -d postgres
Start-Sleep -Seconds 10
dotnet ef database update --project src/WareHouseManagement.Infrastructure --startup-project src/WareHouseManagement.API
```

---

## 📚 დოკუმენტაცია

- 📦 [Docker-ით გაშვება - სრული გაიდი](./DOCKER_START_GUIDE.md)
- 💻 [ლოკალურად გაშვება - სრული გაიდი](./LOCAL_START_GUIDE.md)
- 📋 [README](./README.md) - ზოგადი ინფორმაცია

---

## 🎯 მთავარი Endpoints

```http
GET    /api/companies              # ყველა კომპანია
POST   /api/companies              # ახალი კომპანია
GET    /api/products               # ყველა პროდუქტი
POST   /api/products               # ახალი პროდუქტი
GET    /api/warehouses             # ყველა საწყობი
POST   /api/orders                 # ახალი შეკვეთა
```

**სრული სია:** http://localhost:5000/swagger

---

**წარმატებებს! 🚀**

