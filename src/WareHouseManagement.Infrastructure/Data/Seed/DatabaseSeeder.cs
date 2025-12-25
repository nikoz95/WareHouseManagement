using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // თუ უკვე არის მონაცემები, არ დავსიდოთ
        if (await context.Companies.AnyAsync() || await context.Products.AnyAsync())
        {
            return;
        }

        // 1. მწარმოებლები
        var manufacturers = new List<Manufacturer>
        {
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Name = "საქართველოს ღვინის კომპანია",
                Country = "საქართველო",
                CreatedAt = DateTime.UtcNow
            },
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Name = "ქართული მშრალი ღვინის კომბინატი",
                Country = "საქართველო",
                CreatedAt = DateTime.UtcNow
            }
        };
        await context.Manufacturers.AddRangeAsync(manufacturers);
        await context.SaveChangesAsync();

        // 2. კომპანიები (რესტორანი და ბარი)
        var restaurantCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = "რესტორანი გურმანია",
            TaxId = "123456789",
            CompanyType = CompanyType.Restaurant,
            Phone = "+995-555-123456",
            Email = "info@gurmania.ge",
            CreatedAt = DateTime.UtcNow
        };

        var barCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = "ბარი ვინოთეკა",
            TaxId = "987654321",
            CompanyType = CompanyType.Bar,
            Phone = "+995-555-654321",
            Email = "sales@vinoteka.ge",
            CreatedAt = DateTime.UtcNow
        };

        await context.Companies.AddRangeAsync(new[] { restaurantCompany, barCompany });
        await context.SaveChangesAsync();

        // 3. საწყობები
        var warehouse1 = new Warehouse
        {
            Id = Guid.NewGuid(),
            Name = "ცენტრალური საწყობი",
            CreatedAt = DateTime.UtcNow
        };

        var warehouse2 = new Warehouse
        {
            Id = Guid.NewGuid(),
            Name = "რეგიონალური საწყობი - კახეთი",
            CreatedAt = DateTime.UtcNow
        };

        await context.Warehouses.AddRangeAsync(new[] { warehouse1, warehouse2 });
        await context.SaveChangesAsync();

        // 4. საწყობის ლოკაციები
        var location1 = new WarehouseLocation
        {
            Id = Guid.NewGuid(),
            WarehouseId = warehouse1.Id,
            LocationName = "A სექცია - თეთრი ღვინოები",
            Description = "ცენტრალური საწყობი, პირველი სართული",
            CreatedAt = DateTime.UtcNow
        };

        var location2 = new WarehouseLocation
        {
            Id = Guid.NewGuid(),
            WarehouseId = warehouse1.Id,
            LocationName = "B სექცია - წითელი ღვინოები",
            Description = "ცენტრალური საწყობი, მეორე სართული",
            CreatedAt = DateTime.UtcNow
        };

        var location3 = new WarehouseLocation
        {
            Id = Guid.NewGuid(),
            WarehouseId = warehouse2.Id,
            LocationName = "C სექცია - პრემიუმ",
            Description = "კახეთის საწყობი, სპეციალური სექცია",
            CreatedAt = DateTime.UtcNow
        };

        await context.WarehouseLocations.AddRangeAsync(new[] { location1, location2, location3 });
        await context.SaveChangesAsync();

        // 5. პროდუქტები (10 ღვინო)
        var products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "საფერავი 2020",
                Barcode = "1234567890001",
                Price = 25.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "რქაწითელი 2021",
                Barcode = "1234567890002",
                Price = 20.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "მუკუზანი 2019",
                Barcode = "1234567890003",
                Price = 30.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "კინძმარაული 2020",
                Barcode = "1234567890004",
                Price = 28.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ხვანჭკარა 2021",
                Barcode = "1234567890005",
                Price = 35.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ცინანდალი 2021",
                Barcode = "1234567890006",
                Price = 18.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "გურჯაანი 2020",
                Barcode = "1234567890007",
                Price = 22.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ნაპარეული 2021",
                Barcode = "1234567890008",
                Price = 24.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ალაზნის ველი თეთრი 2021",
                Barcode = "1234567890009",
                Price = 15.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ალაზნის ველი წითელი 2021",
                Barcode = "1234567890010",
                Price = 16.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ვაშლის სიდრი 2023",
                Barcode = "1234567890011",
                Price = 12.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "მსხლის სიდრი 2023",
                Barcode = "1234567890012",
                Price = 13.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ლუდი კეგი - ლაგერი 50L",
                Barcode = "1234567890013",
                Price = 180.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "ლუდი კეგი - IPA 30L",
                Barcode = "1234567890014",
                Price = 150.00m,
                UnitTypeRuleId = (await context.UnitTypeRules.FirstAsync(u => u.UnitType == UnitType.Liter)).Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // 6. პროდუქტების დეტალები
        var productDetailsList = new List<ProductDetails>();
        var alcoholicDetailsList = new List<AlcoholicProductDetails>();
        
        for (int i = 0; i < products.Count; i++)
        {
            // განსაზღვრავთ პროდუქტის ტიპს
            string productType = "ღვინო";
            string countryOfOrigin = "საქართველო";
            
            if (i >= 10 && i <= 11) // სიდრი
            {
                productType = "სიდრი";
            }
            else if (i >= 12) // კეგი
            {
                productType = "ლუდი (კეგი)";
            }
            
            // ჯერ ProductDetails
            var productDetails = new ProductDetails
            {
                Id = Guid.NewGuid(),
                ProductId = products[i].Id,
                CountryOfOrigin = countryOfOrigin,
                ProductType = productType,
                ShelfLifeMonths = i >= 12 ? 6 : 24, // კეგს უფრო მცირე ვადა აქვს
                AdditionalNotes = i < 3 ? "პრემიუმ ხარისხის პროდუქტი" : i >= 12 ? "ახალი კეგი, დალუქული" : "სტანდარტული ხარისხის პროდუქტი",
                CreatedAt = DateTime.UtcNow
            };
            productDetailsList.Add(productDetails);
            
            // შემდეგ AlcoholicProductDetails
            var alcoholicDetails = new AlcoholicProductDetails
            {
                Id = Guid.NewGuid(),
                ProductDetailsId = productDetails.Id,
                AlcoholPercentage = i >= 12 ? 4.5m + (i * 0.2m) : i >= 10 ? 5.5m + (i * 0.3m) : 11.5m + (i * 0.5m), // ლუდი/სიდრი უფრო დაბალი %
                Region = i >= 12 ? "თბილისი" : i >= 10 ? "კახეთი" : "კახეთი",
                ServingTemperature = i >= 10 ? 4.0m : (i < 5 ? 16.0m : 10.0m), // სიდრი და ლუდი ცივი
                QualityClass = i < 3 ? "პრემიუმ" : i >= 12 ? "Craft Beer" : "სტანდარტი",
                CreatedAt = DateTime.UtcNow
            };
            alcoholicDetailsList.Add(alcoholicDetails);
        }

        await context.ProductDetails.AddRangeAsync(productDetailsList);
        await context.SaveChangesAsync();
        
        await context.AlcoholicProductDetails.AddRangeAsync(alcoholicDetailsList);
        await context.SaveChangesAsync();

        // 7. საწყობის მარაგი
        var stocks = new List<WarehouseStock>();
        for (int i = 0; i < products.Count; i++)
        {
            var locationId = i < 5 ? location2.Id : (i < 8 ? location1.Id : location3.Id);
            var manufacturerId = i % 2 == 0 ? manufacturers[0].Id : manufacturers[1].Id;

            var stock = new WarehouseStock
            {
                Id = Guid.NewGuid(),
                WarehouseLocationId = locationId,
                ProductId = products[i].Id,
                ManufacturerId = manufacturerId,
                Quantity = 100 + (i * 10),
                PurchasePrice = products[i].Price * 0.7m,
                ExpirationDate = DateTime.UtcNow.AddYears(2),
                CreatedAt = DateTime.UtcNow
            };

            stocks.Add(stock);

            // შეფუთვის დეტალები
            var packaging = new PackagingDetails
            {
                Id = Guid.NewGuid(),
                WarehouseStockId = stock.Id,
                PackagingType = "Box",
                UnitsPerPackage = 6,
                FullPackagesCount = (int)(stock.Quantity / 6),
                PartialPackagesCount = (int)(stock.Quantity % 6) > 0 ? 1 : 0,
                UnitsInPartialPackages = (int)(stock.Quantity % 6),
                CreatedAt = DateTime.UtcNow
            };

            await context.PackagingDetails.AddAsync(packaging);
        }

        await context.WarehouseStocks.AddRangeAsync(stocks);
        await context.SaveChangesAsync();

        // 8. შეკვეთები (20 შეკვეთა)
        var orders = new List<Order>();
        var random = new Random();

        for (int i = 1; i <= 20; i++)
        {
            var orderDate = DateTime.UtcNow.AddDays(-random.Next(1, 60));
            var isCompleted = i <= 12; // პირველი 12 დასრულებული, დანარჩენი მიმდინარე

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = $"ORD-2024-{i:D4}",
                CompanyId = i % 2 == 0 ? restaurantCompany.Id : barCompany.Id,
                OrderDate = orderDate,
                TotalAmount = 0, // მოგვიანებით გამოვთვლით
                Status = isCompleted ? OrderStatus.Delivered : OrderStatus.Pending,
                CreatedAt = orderDate
            };

            // შეკვეთის პოზიციები (2-5 პროდუქტი თითო შეკვეთაში)
            var itemCount = random.Next(2, 6);
            var orderItems = new List<OrderItem>();

            for (int j = 0; j < itemCount; j++)
            {
                var product = products[random.Next(products.Count)];
                var quantity = random.Next(6, 25);

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    TotalPrice = quantity * product.Price,
                    CreatedAt = orderDate
                };

                orderItems.Add(orderItem);
                order.TotalAmount += orderItem.TotalPrice;
            }

            order.OrderItems = orderItems;
            orders.Add(order);

            // დებიტორი თითოეული შეკვეთისთვის
            var debtor = new Debtor
            {
                Id = Guid.NewGuid(),
                CompanyId = order.CompanyId,
                DebtorName = i % 2 == 0 ? restaurantCompany.Name : barCompany.Name,
                Phone = i % 2 == 0 ? restaurantCompany.Phone : barCompany.Phone,
                Email = i % 2 == 0 ? restaurantCompany.Email : barCompany.Email,
                TotalDebt = order.TotalAmount,
                PaidAmount = isCompleted && random.Next(2) == 0 ? order.TotalAmount : 0,
                RemainingDebt = isCompleted && random.Next(2) == 0 ? 0 : order.TotalAmount,
                DebtDate = orderDate,
                LastPaymentDate = isCompleted && random.Next(2) == 0 ? orderDate.AddDays(random.Next(1, 10)) : null,
                CreatedAt = orderDate
            };

            await context.Debtors.AddAsync(debtor);
        }

        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ ბაზა წარმატებით შეივსო ტესტური მონაცემებით!");
        Console.WriteLine($"   - მწარმოებლები: {manufacturers.Count}");
        Console.WriteLine($"   - კომპანიები: 2");
        Console.WriteLine($"   - საწყობები: 2");
        Console.WriteLine($"   - პროდუქტები: {products.Count}");
        Console.WriteLine($"   - შეკვეთები: {orders.Count}");
    }
}

