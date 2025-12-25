# 📚 დოკუმენტაციის ინდექსი - Warehouse Management System

## 🎯 საიდან დავიწყო?

### პირველად იწყებთ? 🆕
👉 **[QUICK_START.md](./QUICK_START.md)** - სწრაფი სახელმძღვანელო ყველაფრისთვის

### გაშვების ინსტრუქციები 🚀

| გზა | ფაილი | როდის გამოიყენო |
|-----|-------|------------------|
| 🐳 **Docker-ით** | [DOCKER_START_GUIDE.md](./DOCKER_START_GUIDE.md) | Production-like setup, რეკომენდებული |
| 💻 **ლოკალურად** | [LOCAL_START_GUIDE.md](./LOCAL_START_GUIDE.md) | Development, debugging |
| 🐛 **Docker Debugging** | [DOCKER_DEBUG_GUIDE.md](./DOCKER_DEBUG_GUIDE.md) | Debug Docker containers, remote debugging |
| ⚡ **სწრაფი** | [QUICK_START.md](./QUICK_START.md) | სწრაფი reference, cheat sheet |

---

## 📖 დოკუმენტაციის სტრუქტურა

### 1. QUICK_START.md ⚡
**მიზანი:** სწრაფი დაწყება და reference

**შინაარსი:**
- ✅ სწრაფი setup (Docker + ლოკალური)
- ✅ მთავარი ბრძანებები
- ✅ URLs და credentials
- ✅ ხშირი პრობლემები და სწრაფი გადაწყვეტები
- ✅ Cheat sheet

**ვის უნდა წაიკითხოს:**
- პირველად იწყებთ პროექტზე მუშაობას
- გჭირდება სწრაფი reference
- დაგავიწყდა როგორ გავუშვა

---

### 2. DOCKER_START_GUIDE.md 🐳
**მიზანი:** სრული Docker-ით გაშვების გაიდი

**შინაარსი:**
- ✅ Docker-ის დაინსტალირება და setup
- ✅ Step-by-step პირველი გაშვება
- ✅ PostgreSQL + pgAdmin + API setup
- ✅ Docker management ბრძანებები
- ✅ Troubleshooting (დეტალური)
- ✅ Production deployment tips

**ვის უნდა წაიკითხოს:**
- Docker-ით პირველად მუშაობთ
- Production-like environment გჭირდება
- Containerized setup სურთ
- CI/CD pipeline-ში გაშვება გჭირდება

**ძირითადი თემები:**
- Docker Desktop setup
- docker-compose გამოყენება
- კონტეინერების მართვა
- Logs და debugging
- Database migration Docker-ში
- Port conflicts-ის მოგვარება

---

### 3. LOCAL_START_GUIDE.md 💻
**მიზანი:** Docker-ის გარეშე local development

**შინაარსი:**
- ✅ .NET 9.0 SDK setup
- ✅ PostgreSQL ინსტალაცია (ან Docker PostgreSQL)
- ✅ Entity Framework migrations
- ✅ Development tools (dotnet CLI, EF Core)
- ✅ Hot reload და watch mode
- ✅ Database management tools (pgAdmin, DBeaver, psql)
- ✅ IDE setup (VS, Rider, VS Code)

**ვის უნდა წაიკითხოს:**
- Development-ზე მუშაობთ
- Debug გჭირდება
- Hot reload სურთ
- Docker არ გაქვთ ან არ გინდათ გამოყენება

**ძირითადი თემები:**
- dotnet CLI commands
- EF Core migrations management
- Watch mode და hot reload
- Database tools setup
- Development workflow
- IDE configurations

---

### 4. README.md 📋
**მიზანი:** პროექტის overview და quick links

**შინაარსი:**
- ✅ პროექტის აღწერა
- ✅ ტექნოლოგიები
- ✅ არქიტექტურა (Clean Architecture)
- ✅ API endpoints overview
- ✅ ბმულები სხვა გაიდებზე

**ვის უნდა წაიკითხოს:**
- პირველად ხედავთ პროექტს
- გინდათ რომ გაიგოთ პროექტის სტრუქტურა
- API endpoints-ები გაინტერესებთ

---

## 🗺️ Navigation Map

```
პროექტის დაწყება
    ↓
README.md (overview)
    ↓
QUICK_START.md (სწრაფი არჩევანი)
    ↓
არჩევანი:
├── Docker-ით? → DOCKER_START_GUIDE.md
└── ლოკალურად? → LOCAL_START_GUIDE.md
```

---

## 📁 ფაილების სია

### მთავარი დოკუმენტაცია
- `README.md` - პროექტის overview
- `QUICK_START.md` - სწრაფი სახელმძღვანელო
- `DOCKER_START_GUIDE.md` - Docker-ით გაშვება (დეტალური)
- `LOCAL_START_GUIDE.md` - ლოკალურად გაშვება (დეტალური)
- `DOCUMENTATION_INDEX.md` - ეს ფაილი (დოკუმენტაციის ინდექსი)

### Scripts
- `start-docker.ps1` - Docker setup ავტომატიზაცია
- `start-local.ps1` - Local setup ავტომატიზაცია

### Configuration
- `docker-compose.yml` - სრული stack (PostgreSQL + pgAdmin + API)
- `docker-compose.postgres.yml` - მხოლოდ PostgreSQL
- `Dockerfile` - API image
- `appsettings.json` / `appsettings.Development.json` - API კონფიგურაცია

---

## 🎓 სასწავლო გზა

### დამწყებთათვის (პირველი დღე)
1. **[README.md](./README.md)** - გაიგე რა არის პროექტი
2. **[QUICK_START.md](./QUICK_START.md)** - გაუშვი სწრაფად
3. **[DOCKER_START_GUIDE.md](./DOCKER_START_GUIDE.md)** ან **[LOCAL_START_GUIDE.md](./LOCAL_START_GUIDE.md)** - დეტალური setup
4. **http://localhost:5000/swagger** - API-ს გამოცდა

### Developers-თვის
1. **[LOCAL_START_GUIDE.md](./LOCAL_START_GUIDE.md)** - local development setup
2. Development tools section - IDE და tools setup
3. Migration management - database-ის მართვა
4. Development workflow - coding და testing

### DevOps/Deployment-თვის
1. **[DOCKER_START_GUIDE.md](./DOCKER_START_GUIDE.md)** - containerization
2. Production deployment section
3. `docker-compose.yml` - orchestration
4. Environment variables და configuration

---

## 🔍 სპეციფიური თემების პოვნა

### Docker
- Setup: [DOCKER_START_GUIDE.md → წინაპირობები](./DOCKER_START_GUIDE.md#-წინაპირობები)
- პირველი გაშვება: [DOCKER_START_GUIDE.md → დეტალური ინსტრუქცია](./DOCKER_START_GUIDE.md#-დეტალური-ინსტრუქცია)
- Management: [DOCKER_START_GUIDE.md → Docker-ის მართვა](./DOCKER_START_GUIDE.md#️-docker-ის-მართვა)
- Problems: [DOCKER_START_GUIDE.md → ხშირი პრობლემები](./DOCKER_START_GUIDE.md#-ხშირი-პრობლემები)

### Local Development
- Prerequisites: [LOCAL_START_GUIDE.md → წინაპირობები](./LOCAL_START_GUIDE.md#-წინაპირობები)
- PostgreSQL: [LOCAL_START_GUIDE.md → PostgreSQL Setup](./LOCAL_START_GUIDE.md#️⃣-postgresql-setup)
- Migrations: [LOCAL_START_GUIDE.md → Database Migration](./LOCAL_START_GUIDE.md#️⃣-database-migration)
- Tools: [LOCAL_START_GUIDE.md → Development Tools](./LOCAL_START_GUIDE.md#️-development-tools)

### Database
- Migration ბრძანებები: [QUICK_START.md → Migration](./QUICK_START.md#migration)
- pgAdmin setup: [DOCKER_START_GUIDE.md → pgAdmin Setup](./DOCKER_START_GUIDE.md#pgadmin-setup)
- Database tools: [LOCAL_START_GUIDE.md → Database Management Tools](./LOCAL_START_GUIDE.md#-database-management-tools)

### Troubleshooting
- სწრაფი: [QUICK_START.md → რა ვქნა თუ...](./QUICK_START.md#-რა-ვქნა-თუ)
- Docker: [DOCKER_START_GUIDE.md → ხშირი პრობლემები](./DOCKER_START_GUIDE.md#-ხშირი-პრობლემები)
- Local: [LOCAL_START_GUIDE.md → ხშირი პრობლემები](./LOCAL_START_GUIDE.md#-ხშირი-პრობლემები)

### API Testing
- Swagger UI: http://localhost:5000/swagger
- Endpoints: [README.md → API Endpoints](./README.md#-api-endpoints)
- Testing: [LOCAL_START_GUIDE.md → Testing API](./LOCAL_START_GUIDE.md#3️⃣-testing-api)

---

## ❓ FAQ - ხშირად დასმული კითხვები

### რომელი გზა ავირჩიო - Docker თუ Local?

**Docker-ი აირჩიეთ თუ:**
- ✅ პირველად იწყებთ პროექტზე მუშაობას
- ✅ Production-like environment გჭირდება
- ✅ CI/CD-ში გაშვება გჭირდება
- ✅ არ გსურთ PostgreSQL-ის ლოკალურად ინსტალაცია

**Local აირჩიეთ თუ:**
- ✅ Active development-ზე მუშაობთ
- ✅ Debugging გჭირდება
- ✅ Hot reload სურთ
- ✅ უკვე გაქვთ .NET და PostgreSQL

### საიდან დავიწყო?
👉 **[QUICK_START.md](./QUICK_START.md)**

### როგორ გავუშვა Docker-ით?
👉 **[DOCKER_START_GUIDE.md](./DOCKER_START_GUIDE.md)**

### როგორ გავუშვა ლოკალურად?
👉 **[LOCAL_START_GUIDE.md](./LOCAL_START_GUIDE.md)**

### როგორ შევამოწმო API?
👉 http://localhost:5000/swagger

### როგორ გავაკეთო Migration?
👉 [QUICK_START.md → Migration](./QUICK_START.md#migration)

### რა credentials გამოვიყენო?
👉 [QUICK_START.md → Credentials](./QUICK_START.md#-credentials)

### Docker არ მუშაობს
👉 [DOCKER_START_GUIDE.md → ხშირი პრობლემები](./DOCKER_START_GUIDE.md#-ხშირი-პრობლემები)

### Port 5000 დაკავებულია
👉 [QUICK_START.md → Port დაკავებულია](./QUICK_START.md#-port-5000-დაკავებულია)

---

## 🔗 სასარგებლო ბმულები

### ონლაინ რესურსები
- [.NET 9.0 Documentation](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)
- [Docker Documentation](https://docs.docker.com/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

### ბრაუზერში
- **Swagger UI:** http://localhost:5000/swagger
- **pgAdmin:** http://localhost:8080
- **API Health:** http://localhost:5000/health (თუ არსებობს)

---

## 📊 დოკუმენტაციის მატრიცა

| თემა | QUICK_START | DOCKER_GUIDE | LOCAL_GUIDE | README |
|------|-------------|--------------|-------------|--------|
| **Overview** | ⭐ | - | - | ⭐⭐⭐ |
| **Docker Setup** | ⭐ | ⭐⭐⭐ | - | - |
| **Local Setup** | ⭐ | - | ⭐⭐⭐ | - |
| **Quick Commands** | ⭐⭐⭐ | ⭐ | ⭐ | - |
| **Troubleshooting** | ⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ | - |
| **Migration** | ⭐⭐ | ⭐⭐ | ⭐⭐⭐ | - |
| **API Testing** | ⭐ | ⭐ | ⭐⭐ | ⭐⭐ |
| **Architecture** | - | - | - | ⭐⭐⭐ |

⭐⭐⭐ = დეტალური  
⭐⭐ = საშუალო  
⭐ = მოკლე  
\- = არ არის

---

## 🎯 შემდეგი ნაბიჯები

თქვენი მიზნის მიხედვით:

### ახალი Developer
1. ✅ [README.md](./README.md) - პროექტის გაცნობა
2. ✅ [QUICK_START.md](./QUICK_START.md) - გაშვება
3. ✅ [LOCAL_START_GUIDE.md](./LOCAL_START_GUIDE.md) - development setup
4. ✅ http://localhost:5000/swagger - API გამოცდა

### QA/Tester
1. ✅ [QUICK_START.md](./QUICK_START.md) - სწრაფი გაშვება
2. ✅ [DOCKER_START_GUIDE.md](./DOCKER_START_GUIDE.md) - stable environment
3. ✅ http://localhost:5000/swagger - API testing

### DevOps
1. ✅ [DOCKER_START_GUIDE.md](./DOCKER_START_GUIDE.md) - containerization
2. ✅ `docker-compose.yml` - orchestration გაცნობა
3. ✅ Production deployment section

### Project Manager
1. ✅ [README.md](./README.md) - პროექტის overview
2. ✅ API Endpoints section - ფუნქციონალობა

---

**დოკუმენტაციის განახლების თარიღი:** 2024  
**პროექტის ვერსია:** .NET 9.0  
**Database:** PostgreSQL 16  

**მთავარი გაიდები:**
- 📦 [Docker გაშვება](./DOCKER_START_GUIDE.md)
- 💻 [ლოკალური გაშვება](./LOCAL_START_GUIDE.md)
- ⚡ [სწრაფი დაწყება](./QUICK_START.md)

---

**წარმატებებს! 🚀**

