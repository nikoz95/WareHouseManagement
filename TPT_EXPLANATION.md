# 🗄️ Composition Approach - დამოუკიდებელი ცხრილები (არა Inheritance)

## **2 ცალკე დამოუკიდებელი ცხრილი:**

### **1. WarehouseStocks (ყველა პროდუქტისთვის)**
```sql
CREATE TABLE WarehouseStocks (
    Id                    UUID PRIMARY KEY,
    WarehouseLocationId   UUID NOT NULL,
    ProductId             UUID NOT NULL,
    ManufacturerId        UUID NOT NULL,
    Quantity              DECIMAL(18,3) NOT NULL,
    PurchasePrice         DECIMAL(18,2) NOT NULL,
    ExpirationDate        TIMESTAMP NULL,
    CreatedAt             TIMESTAMP NOT NULL,
    UpdatedAt             TIMESTAMP NOT NULL,
    IsDeleted             BOOLEAN NOT NULL,
    
    FOREIGN KEY (WarehouseLocationId) REFERENCES WarehouseLocations(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id)
);
```

### **2. AlcoholicStockDetails (მხოლოდ ალკოჰოლური პროდუქტებისთვის)**
```sql
CREATE TABLE AlcoholicStockDetails (
    Id                    UUID PRIMARY KEY,
    WarehouseStockId      UUID NOT NULL UNIQUE,  -- FK to WarehouseStocks.Id (One-to-One)
    BatchNumber           VARCHAR(50) NULL,
    ExciseStampNumber     VARCHAR(100) NULL,
    CertificateNumber     VARCHAR(100) NULL,
    StorageTemperature    DECIMAL(5,2) NULL,
    CreatedAt             TIMESTAMP NOT NULL,
    UpdatedAt             TIMESTAMP NOT NULL,
    IsDeleted             BOOLEAN NOT NULL,
    
    FOREIGN KEY (WarehouseStockId) REFERENCES WarehouseStocks(Id) ON DELETE CASCADE
);
```

---

## 📊 **მაგალითი - ბაზაში რა ინახება:**

### **ალკოჰოლური პროდუქტის ჩანაწერი:**

**WarehouseStocks ცხრილი:**
```
Id: 'abc-123'
Quantity: 100.5
PurchasePrice: 25.50
...საერთო ველები...
```

**AlcoholicStockDetails ცხრილი:**
```
Id: 'details-456'
WarehouseStockId: 'abc-123'     ← FK to WarehouseStocks
BatchNumber: 'BATCH-2024-001'
ExciseStampNumber: 'EXC-12345'
CertificateNumber: 'CERT-789'
StorageTemperature: 8.5
```

### **ზოგადი პროდუქტის ჩანაწერი:**

**WarehouseStocks ცხრილი:**
```
Id: 'xyz-789'
Quantity: 50.0
PurchasePrice: 10.00
...საერთო ველები...
```

**AlcoholicStockDetails ცხრილი:**
```
(არ არის ჩანაწერი - მხოლოდ ალკოჰოლისთვის)
```

---

## 🔍 **როგორ კითხულობს EF Core:**

### **Query 1: ყველა stock-ის მიღება (БЕЗ AlcoholicDetails)** ✅
```csharp
var allStocks = await context.WarehouseStocks.ToListAsync();
```

**გენერირებული SQL:**
```sql
SELECT * 
FROM WarehouseStocks 
WHERE IsDeleted = false
```
**✅ არ აკეთებს JOIN-ს!** AlcoholicDetails-ის ველები იქნება null.

---

### **Query 2: stock-ები ალკოჰოლის დეტალებით** 📦
```csharp
var stocksWithAlcoholicDetails = await context.WarehouseStocks
    .Include(ws => ws.AlcoholicDetails)
    .ToListAsync();
```

**გენერირებული SQL:**
```sql
SELECT ws.*, ad.*
FROM WarehouseStocks ws
LEFT JOIN AlcoholicStockDetails ad ON ws.Id = ad.WarehouseStockId
WHERE ws.IsDeleted = false
```
**✅ JOIN მხოლოდ როდესაც გჭირდება!**

---

### **Query 3: მხოლოდ ალკოჰოლური stock-ები**
```csharp
var alcoholicStocks = await context.WarehouseStocks
    .Where(ws => ws.AlcoholicDetails != null)
    .Include(ws => ws.AlcoholicDetails)
    .ToListAsync();
```

**გენერირებული SQL:**
```sql
SELECT ws.*, ad.*
FROM WarehouseStocks ws
INNER JOIN AlcoholicStockDetails ad ON ws.Id = ad.WarehouseStockId
WHERE ws.IsDeleted = false AND ad.Id IS NOT NULL
```

---

### **Query 4: მხოლოდ არაალკოჰოლური stock-ები**
```csharp
var generalStocks = await context.WarehouseStocks
    .Where(ws => ws.AlcoholicDetails == null)
    .ToListAsync();
```

**გენერირებული SQL:**
```sql
SELECT ws.*
FROM WarehouseStocks ws
LEFT JOIN AlcoholicStockDetails ad ON ws.Id = ad.WarehouseStockId
WHERE ws.IsDeleted = false AND ad.Id IS NULL
```

---

## ✅ **უპირატესობები:**

1. **✅ სრულიად დამოუკიდებელი ცხრილები** - არა inheritance
2. **✅ JOIN მხოლოდ როცა გჭირდება** - `.Include()` გამოყენებისას
3. **✅ მარტივი SELECT-ები** - `SELECT * FROM WarehouseStocks` ყოველთვის სწრაფია
4. **✅ არ არის NULL ველები** - ალკოჰოლის ველები მხოლოდ AlcoholicStockDetails-ში
5. **✅ NOT NULL constraints** - AlcoholicStockDetails-ის ყველა ველს შეუძლია NOT NULL
6. **✅ მოქნილობა** - მომავალში დაემატოს სხვა Details ცხრილები
7. **✅ Performance** - არ აკეთებს JOIN-ს თუ არ გჭირდება

---

## 🎯 **გამოყენების მაგალითები:**

### **📌 ზოგადი stock-ების სია (სწრაფი)**
```csharp
// არ ჭირდება alcoholic details
var stocks = await _context.WarehouseStocks
    .Where(ws => ws.Quantity > 0)
    .ToListAsync();
// ✅ SQL: SELECT * FROM WarehouseStocks WHERE Quantity > 0
```

### **📌 stock-ების სია ალკოჰოლის ინფოთ (როცა გჭირდება)**
```csharp
// გჭირდება alcoholic details
var detailedStocks = await _context.WarehouseStocks
    .Include(ws => ws.AlcoholicDetails)
    .Where(ws => ws.Quantity > 0)
    .ToListAsync();
// ✅ SQL: SELECT ws.*, ad.* FROM WarehouseStocks ws LEFT JOIN AlcoholicStockDetails ad...
```

### **📌 ახალი ალკოჰოლური stock-ის დამატება**
```csharp
var stock = new WarehouseStock
{
    Id = Guid.NewGuid(),
    Quantity = 100,
    PurchasePrice = 25.50m,
    // ...
    AlcoholicDetails = new AlcoholicStockDetails
    {
        Id = Guid.NewGuid(),
        BatchNumber = "BATCH-001",
        ExciseStampNumber = "EXC-123",
        // ...
    }
};

await _context.WarehouseStocks.AddAsync(stock);
await _context.SaveChangesAsync();
// ✅ ორივე ცხრილში ჩაიწერება
```

### **📌 ზოგადი პროდუქტის დამატება**
```csharp
var stock = new WarehouseStock
{
    Id = Guid.NewGuid(),
    Quantity = 50,
    PurchasePrice = 10.00m,
    // ...
    AlcoholicDetails = null  // ← არ არის ალკოჰოლური
};

await _context.WarehouseStocks.AddAsync(stock);
await _context.SaveChangesAsync();
// ✅ მხოლოდ WarehouseStocks ცხრილში ჩაიწერება
```

---

## 📝 **შედარება:**

| მახასიათებელი | TPH | TPT | **Composition ✅** |
|--------------|-----|-----|--------------------|
| ცხრილების რაოდენობა | 1 | 3 | 2 |
| NULL ველები | ბევრი ❌ | არ არის ✅ | არ არის ✅ |
| JOIN როცა არ გჭირდება | არა ✅ | ყოველთვის ❌ | **არა ✅✅** |
| JOIN როცა გჭირდება | არა | ავტომატური | `.Include()` ✅ |
| Performance (simple query) | სწრაფი ✅ | ნელი ❌ | **სწრაფი ✅✅** |
| Performance (detailed query) | სწრაფი ✅ | საშუალო | საშუალო ✅ |
| სქემის სისუფთავე | ნაკლებად | კარგი ✅ | **ძალიან კარგი ✅✅** |
| NOT NULL constraints | შეუძლებელია ❌ | შესაძლებელია ✅ | **შესაძლებელია ✅✅** |
| მოქნილობა | ნაკლებად | საშუალო | **ძალიან მოქნილი ✅✅** |

---

## 🎯 **თქვენი არჩევანი:**

**✅ Composition (One-to-One Optional Relationship)**  
- 2 დამოუკიდებელი ცხრილი
- JOIN მხოლოდ როცა `.Include()` გამოიძახე
- ყველაზე მოქნილი და ეფექტური მიდგომა

---

## 🚀 **სამომავლო გაფართოება:**

მომავალში თუ დაგჭირდა სხვა ტიპის პროდუქტები:

```csharp
// უბრალოდ დაამატე ახალი Details ცხრილი
public class PerishableStockDetails : BaseEntity
{
    public Guid WarehouseStockId { get; set; }
    public DateTime BestBeforeDate { get; set; }
    public decimal OptimalTemperature { get; set; }
    
    public WarehouseStock WarehouseStock { get; set; } = null!;
}

// და WarehouseStock-ში:
public class WarehouseStock : BaseEntity
{
    // ...existing code...
    public AlcoholicStockDetails? AlcoholicDetails { get; set; }
    public PerishableStockDetails? PerishableDetails { get; set; }
    // შეიძლება რამდენიც გინდა!
}
```

**✅ არა inheritance, არამედ composition - მაქსიმალური მოქნილობა!**


### **1. WarehouseStocks (ბაზისური ცხრილი)**
```sql
CREATE TABLE WarehouseStocks (
    Id                    UUID PRIMARY KEY,
    WarehouseLocationId   UUID NOT NULL,
    ProductId             UUID NOT NULL,
    ManufacturerId        UUID NOT NULL,
    Quantity              DECIMAL(18,3) NOT NULL,
    PurchasePrice         DECIMAL(18,2) NOT NULL,
    ExpirationDate        TIMESTAMP NULL,
    CreatedAt             TIMESTAMP NOT NULL,
    UpdatedAt             TIMESTAMP NOT NULL,
    IsDeleted             BOOLEAN NOT NULL,
    
    FOREIGN KEY (WarehouseLocationId) REFERENCES WarehouseLocations(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id)
);
```

### **2. AlcoholicWarehouseStocks (ალკოჰოლური პროდუქტების ცხრილი)**
```sql
CREATE TABLE AlcoholicWarehouseStocks (
    Id                    UUID PRIMARY KEY,  -- FK to WarehouseStocks.Id
    BatchNumber           VARCHAR(50) NULL,
    ExciseStampNumber     VARCHAR(100) NULL,
    CertificateNumber     VARCHAR(100) NULL,
    StorageTemperature    DECIMAL(5,2) NULL,
    
    FOREIGN KEY (Id) REFERENCES WarehouseStocks(Id) ON DELETE CASCADE
);
```

### **3. GeneralWarehouseStocks (ზოგადი პროდუქტების ცხრილი)**
```sql
CREATE TABLE GeneralWarehouseStocks (
    Id                    UUID PRIMARY KEY,  -- FK to WarehouseStocks.Id
    Notes                 VARCHAR(500) NULL,
    
    FOREIGN KEY (Id) REFERENCES WarehouseStocks(Id) ON DELETE CASCADE
);
```

---

## 📊 **მაგალითი - ბაზაში რა ინახება:**

### **ალკოჰოლური პროდუქტის ჩანაწერი:**

**WarehouseStocks ცხრილი:**
```
Id: 'abc-123'
Quantity: 100.5
PurchasePrice: 25.50
...საერთო ველები...
```

**AlcoholicWarehouseStocks ცხრილი:**
```
Id: 'abc-123'                    ← იგივე ID (Foreign Key)
BatchNumber: 'BATCH-2024-001'
ExciseStampNumber: 'EXC-12345'
CertificateNumber: 'CERT-789'
StorageTemperature: 8.5
```

### **ზოგადი პროდუქტის ჩანაწერი:**

**WarehouseStocks ცხრილი:**
```
Id: 'xyz-456'
Quantity: 50.0
PurchasePrice: 10.00
...საერთო ველები...
```

**GeneralWarehouseStocks ცხრილი:**
```
Id: 'xyz-456'                    ← იგივე ID (Foreign Key)
Notes: 'Handle with care'
```

---

## 🔍 **როგორ კითხულობს EF Core:**

### **Query 1: ყველა stock-ის მიღება**
```csharp
var allStocks = await context.WarehouseStocks.ToListAsync();
```

**გენერირებული SQL:**
```sql
-- ალკოჰოლური stock-ებისთვის:
SELECT ws.*, aws.*
FROM WarehouseStocks ws
INNER JOIN AlcoholicWarehouseStocks aws ON ws.Id = aws.Id
WHERE ws.IsDeleted = false

UNION ALL

-- ზოგადი stock-ებისთვის:
SELECT ws.*, gws.*
FROM WarehouseStocks ws
INNER JOIN GeneralWarehouseStocks gws ON ws.Id = gws.Id
WHERE ws.IsDeleted = false
```

### **Query 2: მხოლოდ ალკოჰოლური**
```csharp
var alcoholicStocks = await context.WarehouseStocks
    .OfType<AlcoholicWarehouseStock>()
    .ToListAsync();
```

**გენერირებული SQL:**
```sql
SELECT ws.*, aws.*
FROM WarehouseStocks ws
INNER JOIN AlcoholicWarehouseStocks aws ON ws.Id = aws.Id
WHERE ws.IsDeleted = false
```

---

## ✅ **უპირატესობები:**

1. **✅ სრულიად ცალკე ცხრილები** - თითოეული ტიპი დამოუკიდებელია
2. **✅ არ არის NULL ველები** - მხოლოდ საჭირო ველები თითოეულ ცხრილში
3. **✅ NOT NULL constraints** - ტიპ-სპეციფიკურ ველებზე შეგიძლიათ NOT NULL
4. **✅ მოწესრიგებული სქემა** - ნორმალიზებული სტრუქტურა
5. **✅ მომავალში ადვილი გაფართოება** - უბრალოდ დაამატე ახალი ცხრილი

---

## ⚠️ **ნაკლოვანებები:**

1. **❌ JOIN-ები** - ყოველი query-ში სჭირდება JOIN
2. **❌ ნელი performance** - მეტი ცხრილი = მეტი JOIN
3. **❌ რთული INSERT** - 2 ცხრილში უნდა ჩაწეროს

---

## 💡 **როდის გამოვიყენოთ TPT:**

- ✅ თუ ტიპებს აქვთ **ბევრი განსხვავებული ველი**
- ✅ თუ **performance არ არის კრიტიკული**
- ✅ თუ გვინდა **სუფთა, ნორმალიზებული სქემა**
- ✅ თუ გვინდა **NOT NULL constraints** ტიპ-სპეციფიკურ ველებზე

---

## 📝 **შედარება TPH vs TPT:**

| მახასიათებელი | TPH (1 ცხრილი) | TPT (3 ცხრილი) ✅ |
|--------------|-----------------|-------------------|
| ცხრილების რაოდენობა | 1 | 3 |
| NULL ველები | ბევრი ❌ | არ არის ✅ |
| JOIN-ების რაოდენობა | 0 ✅ | ყოველ query-ში ❌ |
| Performance | სწრაფი ✅ | ნელი ❌ |
| სქემის სისუფთავე | ნაკლებად ✅ | ძალიან სუფთა ✅ |
| NOT NULL constraints | შეუძლებელია ❌ | შესაძლებელია ✅ |

---

## 🎯 **თქვენი არჩევანი:**

**TPT (Table Per Type)** - სრულიად დამოუკიდებელი ცხრილები! ✅

ახლა თითოეული ტიპი თავის ცხრილში ცხოვრობს და სრულიად დამოუკიდებელია! 🎉

---

**კონფიგურაცია:** `UseTptMappingStrategy()`  
**შედეგი:** 3 ცალკე ცხრილი ბაზაში

