# 📡 API Testing Guide

Warehouse Management System-ის API endpoints-ების ტესტირების სახელმძღვანელო.

## 🌐 Base URL

```
http://localhost:5000
```

ყველა endpoint იწყება `/api/` prefix-ით და იყენებს **lowercase kebab-case** სტილს.

---

## 🧪 Quick Test Flow

### 1️⃣ Companies (კომპანიები)

#### მიიღე ყველა კომპანია
```http
GET /api/companies
```

**Response:**
```json
[
  {
    "id": "guid",
    "name": "ღვინის კომპანია ABC",
    "taxId": "123456789",
    "email": "info@wineabc.ge",
    "phone": "+995 32 2 123456",
    "address": "თბილისი, ვაჟა-ფშაველას 45",
    "companyType": 0,
    "isPartner": true,
    "createdAt": "2024-01-01T..."
  }
]
```

#### შექმენი ახალი კომპანია
```http
POST /api/companies
Content-Type: application/json

{
  "name": "ტესტ კომპანია",
  "taxId": "987654321",
  "email": "test@company.ge",
  "phone": "+995 555 123456",
  "address": "თბილისი",
  "companyType": 0,
  "isPartner": true
}
```

---

### 2️⃣ Products (პროდუქტები)

#### მიიღე ყველა პროდუქტი
```http
GET /api/products
```

#### შექმენი ახალი პროდუქტი (ალკოჰოლური)
```http
POST /api/products
Content-Type: application/json

{
  "name": "Saperavi Reserve 2020",
  "description": "ქართული წითელი მშრალი ღვინო",
  "barcode": "1234567890123",
  "price": 45.50,
  "unitTypeRuleId": "guid-from-unit-type-rules",
  "alcoholicDetails": {
    "alcoholPercentage": 13.5,
    "volume": 0.75,
    "color": "Red",
    "sugarContent": 4.5,
    "manufacturer": "Kakheti Winery",
    "region": "Kakheti",
    "grapeVariety": "Saperavi",
    "servingTemperature": "16-18°C",
    "qualityClass": "Premium"
  }
}
```

#### შექმენი ახალი პროდუქტი (სიდრი)
```http
POST /api/products
Content-Type: application/json

{
  "name": "Georgian Cider Apple",
  "description": "ქართული ვაშლის სიდრი",
  "barcode": "9876543210987",
  "price": 8.50,
  "unitTypeRuleId": "guid-from-unit-type-rules",
  "alcoholicDetails": {
    "alcoholPercentage": 5.0,
    "volume": 0.5,
    "color": "Golden",
    "sugarContent": 12.0,
    "manufacturer": "Cider House",
    "region": "Adjara"
  }
}
```

---

### 3️⃣ Warehouses (საწყობები)

#### მიიღე ყველა საწყობი
```http
GET /api/warehouses
```

#### შექმენი ახალი საწყობი
```http
POST /api/warehouses
Content-Type: application/json

{
  "name": "Main Warehouse Tbilisi",
  "location": "Tbilisi, Industrial Zone 1",
  "capacity": 10000
}
```

---

### 4️⃣ Warehouse Locations (საწყობის ლოკაციები)

#### მიიღე საწყობის ლოკაციები
```http
GET /api/warehouses/{warehouseId}/locations
```

#### დაამატე ლოკაცია საწყობში
```http
POST /api/warehouses/{warehouseId}/locations
Content-Type: application/json

{
  "section": "A",
  "position": "01",
  "capacity": 500
}
```

---

### 5️⃣ Warehouse Stocks (მარაგი)

#### მიიღე მარაგი (ყველა)
```http
GET /api/warehouse-stocks
```

#### მიიღე მარაგი ფილტრებით
```http
GET /api/warehouse-stocks?productId={guid}&includeAlcoholicDetails=true
```

#### დაამატე მარაგი
```http
POST /api/warehouse-stocks
Content-Type: application/json

{
  "warehouseLocationId": "guid",
  "productId": "guid",
  "manufacturerId": "guid",
  "totalUnitsCount": 240,
  "packagingDetails": {
    "unitsPerPackage": 12,
    "totalPackages": 20,
    "partialPackageUnits": 0
  }
}
```

**Explanation:**
- `totalUnitsCount: 240` - სულ 240 ბოთლი
- `unitsPerPackage: 12` - 12 ბოთლი თითო ყუთში
- `totalPackages: 20` - 20 სრული ყუთი (20 × 12 = 240)
- `partialPackageUnits: 0` - არ არის ნახევრად შევსებული ყუთი

---

### 6️⃣ Orders (შეკვეთები)

#### შექმენი შეკვეთა
```http
POST /api/orders
Content-Type: application/json

{
  "orderNumber": "ORD-2024-001",
  "orderDate": "2024-12-26T10:00:00Z",
  "companyId": "guid-of-customer-company",
  "orderStatus": 0,
  "notes": "სასწრაფო შეკვეთა",
  "orderItems": [
    {
      "productId": "guid",
      "warehouseLocationId": "guid",
      "quantity": 24,
      "unitPrice": 45.50
    }
  ]
}
```

**OrderStatus enum:**
- `0` = Pending (მიმდინარე)
- `1` = Completed (დასრულებული)
- `2` = Cancelled (გაუქმებული)

---

### 7️⃣ Stock History (ისტორია)

#### მიიღე ტრანზაქციების ისტორია
```http
GET /api/warehouse-stocks/history
```

#### ფილტრებით
```http
GET /api/warehouse-stocks/history?productId={guid}&fromDate=2024-01-01&toDate=2024-12-31&transactionType=0
```

**TransactionType enum:**
- `0` = StockIn (შემოსვლა)
- `1` = StockOut (გასვლა)
- `2` = Adjustment (კორექტირება)

---

### 8️⃣ Manufacturers (მწარმოებლები)

#### მიიღე ყველა მწარმოებელი
```http
GET /api/manufacturers
```

#### დაამატე ახალი
```http
POST /api/manufacturers
Content-Type: application/json

{
  "name": "Kakheti Premium Wines",
  "country": "Georgia",
  "region": "Kakheti"
}
```

---

### 9️⃣ Unit Type Rules (საზომი ერთეულები)

#### მიიღე წესები
```http
GET /api/unit-type-rules?onlyActive=true
```

#### დაამატე ახალი წესი
```http
POST /api/unit-type-rules
Content-Type: application/json

{
  "unitType": 0,
  "allowDecimal": false,
  "minValue": 1,
  "maxValue": 1000,
  "isActive": true
}
```

**UnitType enum:**
- `0` = Piece (ცალი)
- `1` = Liter (ლიტრი)
- `2` = Kilogram (კილოგრამი)

---

## 🧪 Swagger UI

ყველა endpoint-ს შეგიძლია ტესტირება Swagger-ით:

```
http://localhost:5000/swagger
```

### Swagger-ში ტესტირება:

1. **გახსენი endpoint** - დააჭირე GET/POST/DELETE ღილაკს
2. **"Try it out"** - დააჭირე ამ ღილაკს
3. **შეიყვანე parameters** - თუ საჭიროა
4. **"Execute"** - გაუშვი request
5. **ნახე Response** - ქვემოთ გამოჩნდება პასუხი

---

## 📊 Response Codes

### Success (წარმატება)
- **200 OK** - წარმატებით მიღებულია მონაცემები
- **201 Created** - წარმატებით შეიქმნა ახალი ჩანაწერი
- **204 No Content** - წარმატებით წაიშალა

### Error (შეცდომა)
- **400 Bad Request** - არასწორი მონაცემები
- **404 Not Found** - არ მოიძებნა
- **500 Internal Server Error** - სერვერის შეცდომა

---

## 📝 Example Workflow

### სრული ფლოუ: პროდუქტის დამატება → საწყობში განთავსება → შეკვეთა

#### 1. შექმენი კომპანია (მომწოდებელი)
```http
POST /api/companies
{ "name": "Wine Supplier LLC", "companyType": 0, ... }
```
↓ მიიღე `companyId`

#### 2. შექმენი მწარმოებელი
```http
POST /api/manufacturers
{ "name": "Kakheti Winery", ... }
```
↓ მიიღე `manufacturerId`

#### 3. შექმენი პროდუქტი
```http
POST /api/products
{ "name": "Saperavi 2020", "unitTypeRuleId": "...", ... }
```
↓ მიიღე `productId`

#### 4. შექმენი საწყობი
```http
POST /api/warehouses
{ "name": "Main Warehouse", ... }
```
↓ მიიღე `warehouseId`

#### 5. დაამატე ლოკაცია საწყობში
```http
POST /api/warehouses/{warehouseId}/locations
{ "section": "A", "position": "01", ... }
```
↓ მიიღე `warehouseLocationId`

#### 6. დაამატე მარაგი
```http
POST /api/warehouse-stocks
{
  "warehouseLocationId": "...",
  "productId": "...",
  "manufacturerId": "...",
  "totalUnitsCount": 240
}
```

#### 7. შექმენი შეკვეთა (კლიენტისთვის)
```http
POST /api/orders
{
  "companyId": "customer-company-id",
  "orderItems": [
    { "productId": "...", "quantity": 24, "unitPrice": 45.50 }
  ]
}
```

✅ **მზადაა!** შეკვეთა შეიქმნა და მარაგი ავტომატურად შემცირდა.

---

## 💡 Tips

1. **Swagger გამოიყენე Development-ისთვის** - ყველაზე მარტივი გზაა
2. **Postman Collection** - შეგიძლია შექმნა შენთვის სასურველი requests
3. **Response-ები შეინახე** - ID-ები დაგჭირდება შემდეგი requests-ისთვის
4. **Seed Data გამოიყენე** - უკვე არის 10+ პროდუქტი და კომპანიები

---

## 🔍 Filtering Examples

### Products by price
```http
GET /api/products?minPrice=10&maxPrice=50
```

### Stocks by warehouse
```http
GET /api/warehouse-stocks?warehouseLocationId={guid}
```

### History by date range
```http
GET /api/warehouse-stocks/history?fromDate=2024-01-01&toDate=2024-12-31
```

### Debtors by company
```http
GET /api/debtors?companyId={guid}
```

---

**მზადაა ტესტირებისთვის! 🚀**

