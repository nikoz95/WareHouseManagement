# 🎉 WareHouse Management - Docker Setup Complete!

## ✅ რა შეიქმნა

### Docker Configuration Files:
1. **docker-compose.yml** - სრული setup (PostgreSQL + API)
2. **docker-compose.postgres.yml** - მხოლოდ PostgreSQL
3. **Dockerfile** - API-ის Docker image
4. **.dockerignore** - Docker build optimization

### Startup Scripts:
1. **start-quick.ps1** - ავტომატური PowerShell სკრიპტი (რეკომენდებული)
2. **start.bat** - მარტივი batch სკრიპტი
3. **start-docker.ps1** - სრული Docker Compose გაშვება

### Documentation:
1. **DOCKER_GUIDE.md** - სრული Docker გაიდი
2. **DOCKER_SETUP.md** - Docker setup დოკუმენტაცია
3. **README.md** - განახლებული Docker ინსტრუქციებით

### Configuration Updates:
1. **appsettings.Development.json** - განახლებული connection string
2. **Database migrations** - უკვე შექმნილია `InitialCreate` მიგრაცია

---

## 🚀 როგორ გავუშვა

### ყველაზე მარტივი გზა:
```powershell
.\start-quick.ps1
```

ეს სკრიპტი ყველაფერს გააკეთებს:
- ✅ PostgreSQL-ს Docker-ში გაუშვებს
- ✅ ბაზას შექმნის
- ✅ მიგრაციებს გაუშვებს
- ✅ API-ს გაუშვებს (თუ დაეთანხმე)

### ან ხელით:
```powershell
# 1. PostgreSQL-ის გაშვება Docker-ში
docker-compose -f docker-compose.postgres.yml up -d

# 2. მიგრაციების გაშვება
cd src\WareHouseManagement.API
dotnet ef database update

# 3. API-ის გაშვება
dotnet run
```

### ან ყველაფერი Docker-ში:
```powershell
docker-compose up --build
```

---

## 📊 ბაზის მონაცემები

```
Host:     localhost
Port:     5432
Database: WareHouseManagementDb
Username: warehouse_user
Password: warehouse_pass_2024
```

**Connection String:**
```
Host=localhost;Port=5432;Database=WareHouseManagementDb;Username=warehouse_user;Password=warehouse_pass_2024
```

---

## 🎯 შემდეგი ნაბიჯები

1. **დარწმუნდი რომ Docker Desktop გაშვებულია**
2. **გაუშვი სკრიპტი:** `.\start-quick.ps1`
3. **გახსენი Swagger:** http://localhost:5000/swagger
4. **დაიწყე API-ის ტესტირება!**

---

## 📚 დეტალური დოკუმენტაცია

- **DOCKER_GUIDE.md** - სრული Docker Setup გაიდი
- **README.md** - პროექტის ზოგადი დოკუმენტაცია
- **DOCKER_SETUP.md** - Docker-ის გამოყენების ინსტრუქციები

---

## 🐛 Troubleshooting

### Docker არ გაშვებულა?
```
Error: error during connect...
```
**გადაწყვეტა:** გაუშვი Docker Desktop

### PostgreSQL არ უკავშირდება?
```
Failed to connect to 127.0.0.1:5432
```
**გადაწყვეტა:** დაელოდე 10-15 წამი PostgreSQL-ის სრულ ჩატვირთვას

### Port დაკავებულია?
**გადაწყვეტა:** შეცვალე `docker-compose.yml`-ში:
```yaml
ports:
  - "5433:5432"  # 5432-ის ნაცვლად
```

---

## ✅ ყველაფერი მუშაობს თუ:

- [ ] Docker Desktop გაშვებულია
- [ ] `docker ps` აჩვენებს `warehouse_postgres` container-ს
- [ ] მიგრაციები წარმატებით გადის
- [ ] API გაშვებულია და პასუხობს
- [ ] Swagger UI იხსნება: http://localhost:5000/swagger

თ�� ყველა ✅, გილოცავ! 🎉 პროექტი Docker-ზე მუშაობს!

