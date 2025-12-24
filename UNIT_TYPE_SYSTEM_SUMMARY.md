# პროდუქტის საზომი ერთეულების მოქნილი სისტემა

## განხორციელებული ცვლილებები - სრული მიმოხილვა

### 📋 მთავარი კონცეფცია

შექმნილია **მოქნილი სისტემა** პროდუქტების საზომი ერთეულების მართვისთვის. ახლა საზომი ერთეულების წესები და კონფიგურაციები ინახება ბაზაში და შეიძლება მოქნილად დაემატოს ახალი ერთეულები ადმინისტრატორის მიერ.

---

## 🗂️ ახალი Entity: UnitTypeRule

ცალკე ცხრილი საზომი ერთეულების წესების შესანახად:

```csharp
public class UnitTypeRule
{
    public UnitType UnitType { get; set; }           // საზომი ერთეულის ტიპი
    public string NameKa { get; set; }               // სახელი ქართულად
    public string NameEn { get; set; }               // სახელი ინგლისურად
    public string Abbreviation { get; set; }         // შეკვეცილი ფორმა (ც, ლ, კგ)
    public bool AllowOnlyWholeNumbers { get; set; }  // მხოლოდ მთელი რიცხვები
    public decimal? MinValue { get; set; }           // მინიმალური მნიშვნელობა
    public decimal? MaxValue { get; set; }           // მაქსიმალური მნიშვნელობა
    public decimal? DefaultValue { get; set; }       // ნაგულისხმევი
    public bool IsActive { get; set; }               // აქტიურია თუ არა
    public string? Description { get; set; }         // აღწერა
}
```

### ნაგულისხმევი მონაცემები:

| UnitType | NameKa | Abbreviation | AllowOnlyWholeNumbers | MinValue | MaxValue |
|----------|--------|--------------|----------------------|----------|----------|
| Piece | ცალი | ც | ✅ true | 1 | - |
| Liter | ლიტრი | ლ | ❌ false | 0.001 | 1000 |
| Milliliter | მილილიტრი | მლ | ❌ false | 1 | 10000 |
| Kilogram | კილოგრამი | კგ | ❌ false | 0.001 | 1000 |
| Gram | გრამი | გ | ❌ false | 1 | 100000 |
| Box | ყუთი | ყთ | ✅ true | 1 | - |
| Package | პაკეტი | პქ | ✅ true | 1 | - |

---

## 📦 Product Entity ცვლილებები

### ❌ წაშლილი ველები:
- `BottlesPerBox` (int) - გადატანილია WarehouseStock-ში

### ✅ დამატებული ველები:
- `UnitType` (enum) - საზომი ერთეული (ნაგულისხმევი: Piece)
- `UnitValue` (decimal?) - საზომი რაოდენობა (nullable)

---

## 🏭 WarehouseStock Entity ცვლილებები

### ✅ დამატებული ველი:
- `BottlesPerBox` (int) - ყუთში ბოთლების რაოდენობა
  - **მიზეზი**: ეს პარამეტრი დამოკიდებულია კონკრეტულ პარტიაზე/შეფუთვაზე, არა პროდუქტზე

---

## 📝 OrderItem Entity ცვლილებები

### ✅ დამატებული ველი:
- `BottlesPerBox` (int) - შეკვეთის დროს შენახული მნიშვნელობა
  - **მიზეზი**: იმისთვის რომ ისტორიულად შენახული იყოს შეკვეთის დროს რა იყო BottlesPerBox

---

## ✔️ ვალიდაცია

### CreateProductCommandValidator

ვალიდატორი **დინამიურად** იღებს წესებს ბაზიდან:

1. **UnitType შემოწმება** - უნდა იყოს აქტიური და ხელმისაწვდომი
2. **მთელი რიცხვის შემოწმება** - თუ `AllowOnlyWholeNumbers = true`, მაშინ UnitValue უნდა იყოს მთელი
3. **მინიმუმი/მაქსიმუმი** - ამოწმებს MinValue და MaxValue-ს

**მაგალითები:**
- ✅ `{ unitType: "Piece", unitValue: 1 }` - სწორია
- ❌ `{ unitType: "Piece", unitValue: 1.5 }` - არასწორია (მთელი უნდა იყოს)
- ✅ `{ unitType: "Liter", unitValue: 0.5 }` - სწორია
- ❌ `{ unitType: "Liter", unitValue: 0.0001 }` - არასწორია (min: 0.001)

---

## 🔄 Repository ცვლილებები

### ახალი Repository: IUnitTypeRuleRepository

```csharp
public interface IUnitTypeRuleRepository : IGenericRepository<UnitTypeRule>
{
    Task<UnitTypeRule?> GetByUnitTypeAsync(UnitType unitType);
    Task<IEnumerable<UnitTypeRule>> GetActiveRulesAsync();
}
```

დამატებულია `IUnitOfWork`-ში:
```csharp
IUnitTypeRuleRepository UnitTypeRules { get; }
```

---

## 📊 API გამოყენება

### პროდუქტის შექმნა

```http
POST /api/products
Content-Type: application/json

{
  "name": "ღვინო საფერავი",
  "description": "წითელი მშრალი ღვინო",
  "barcode": "123456789",
  "alcoholPercentage": 12.5,
  "unitType": 1,          // 0=Piece, 1=Liter, 2=Milliliter, და ა.შ.
  "unitValue": 0.75       // 0.75 ლიტრი
}
```

### მაგალითები სხვადასხვა UnitType-ით:

#### 1️⃣ ბოთლი ცალობით
```json
{
  "name": "პივო ნატახტარი",
  "unitType": 0,     // Piece
  "unitValue": 1,    // 1 ცალი
  "alcoholPercentage": 4.5
}
```

#### 2️⃣ ბოთლი ლიტრობით
```json
{
  "name": "ღვინო კინძმარაული",
  "unitType": 1,     // Liter
  "unitValue": 0.75, // 0.75 ლიტრი
  "alcoholPercentage": 11.0
}
```

#### 3️⃣ ყუთობით
```json
{
  "name": "პივო ყუთით",
  "unitType": 5,     // Box
  "unitValue": 1,    // 1 ყუთი (ყუთში ბოთლების რაოდენობა შეინახება საწყობში)
  "alcoholPercentage": 5.0
}
```

---

## 🏗️ მიგრაციები

### ⚠️ მნიშვნელოვანი!

**არ გაუშვათ მიგრაციები** სანამ არ გეტყვით! 

როდესაც მზად იქნებით:

```bash
# მიგრაციის შექმნა (უკვე შექმნილია)
cd src/WareHouseManagement.Infrastructure
dotnet ef migrations add AddUnitTypeSystemAndRemoveBottlesPerBox --startup-project ../WareHouseManagement.API

# ბაზაში გაშვება
dotnet ef database update --startup-project ../WareHouseManagement.API
```

### მიგრაცია ასრულებს:

**Products table:**
- ❌ Drops `BottlesPerBox` column
- ➕ Adds `UnitType` column (integer, default: 0)
- ➕ Adds `UnitValue` column (decimal, nullable)

**WarehouseStocks table:**
- ➕ Adds `BottlesPerBox` column (integer, default: 6)

**OrderItems table:**
- ➕ Adds `BottlesPerBox` column (integer, default: 6)

**UnitTypeRules table:**
- ➕ Creates new table with seed data (7 records)

---

## ✨ უპირატესობები

### 1. **მოქნილობა**
- ახალი საზომი ერთეულების დამატება ხდება ბაზაში, კოდის ცვლილების გარეშე
- წესების მოდიფიცირება რეალურ დროში

### 2. **ვალიდაცია**
- ცენტრალიზებული წესები ბაზაში
- დინამიური ვალიდაცია მონაცემების მიხედვით
- მკაფიო შეცდომის შეტყობინებები

### 3. **ისტორიულობა**
- OrderItem ინახავს BottlesPerBox-ს შეკვეთის დროს
- WarehouseStock ინახავს BottlesPerBox-ს პარტიის მიხედვით

### 4. **კონსისტენტურობა**
- Product აღარ აერია საწყობის სპეციფიკური ინფორმაციით
- თითოეული entity პასუხისმგებელია საკუთარ მონაცემებზე

---

## 🎯 შემდეგი ნაბიჯები

1. ✅ კოდი მზადაა და აშენებულია
2. ⏳ მიგრაციის გაშვება (როდესაც გეტყვით)
3. 🧪 ტესტირება
4. 📱 Frontend-ის განახლება UnitType dropdown-ით

---

## 📞 დამატებითი ფუნქციონალობა

თუ მომავალში გინდათ, შეგიძლიათ დაამატოთ:

- Admin Panel UnitTypeRule-ების მართვისთვის
- კონვერტაციის წესები (მაგ: 1000გ = 1კგ)
- მრავალენოვანი მხარდაჭერა (ქარ/ინგ)
- Custom ერთეულების დამატება

---

**შექმნა თარიღი:** 2024-12-25  
**სტატუსი:** ✅ მზადაა ტესტირებისთვის (მიგრაცია ჯერ არ გაშვებულა)

