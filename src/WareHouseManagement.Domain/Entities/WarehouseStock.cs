﻿﻿using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// საწყობის მარაგი - ზოგადი (ყველა ტიპის პროდუქტისთვის)
/// არა inheritance, არამედ composition
/// </summary>
public class WarehouseStock : BaseEntity
{
    public Guid WarehouseLocationId { get; set; }
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// მწარმოებელი (optional - შეიძლება იყოს უცნობი)
    /// თითოეული WarehouseStock ჩანაწერი წარმოადგენს კონკრეტული მწარმოებლის პროდუქტს.
    /// იგივე პროდუქტი, სხვადასხვა მწარმოებლისგან = სხვადასხვა WarehouseStock ჩანაწერები
    /// </summary>
    public Guid? ManufacturerId { get; set; }
    
    /// <summary>
    /// საწყობში არსებული რაოდენობა (პროდუქტის UnitTypeRule-ის მიხედვით)
    /// </summary>
    public decimal Quantity { get; set; }
    
    public decimal PurchasePrice { get; set; } // შესყიდვის ფასი
    public DateTime? ExpirationDate { get; set; }
    
    // Navigation properties
    public WarehouseLocation WarehouseLocation { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public Manufacturer? Manufacturer { get; set; } // nullable - შეიძლება არ იყოს მითითებული
    
    /// <summary>
    /// Optional: შეფუთვის დეტალები (ყუთები, პაკეტები და ა.შ.)
    /// One-to-One relationship (optional)
    /// გამოიყენება ნებისმიერი პროდუქტისთვის, რომელსაც სჭირდება ყუთების მართვა
    /// </summary>
    public PackagingDetails? PackagingDetails { get; set; }
    
    /// <summary>
    /// Optional: ალკოჰოლური პროდუქტის დამატებითი ინფორმაცია
    /// One-to-One relationship (optional)
    /// მხოლოდ ალკოჰოლური პროდუქტებისთვის
    /// </summary>
    public AlcoholicStockDetails? AlcoholicDetails { get; set; }
    
    /// <summary>
    /// მარაგის ცვლილებების ისტორია
    /// One-to-Many relationship
    /// </summary>
    public ICollection<WarehouseStockHistory> StockHistories { get; set; } = new List<WarehouseStockHistory>();
}

/// <summary>
/// შეფუთვის/პაკეტირების დეტალები (უნივერსალური - ნებისმიერი პროდუქტისთვის)
/// ცალკე ცხრილი - ყუთების, პაკეტების, კოლოფების მართვისთვის
/// One-to-One with WarehouseStock (optional)
/// </summary>
public class PackagingDetails : BaseEntity
{
    public Guid WarehouseStockId { get; set; } // FK to WarehouseStock
    
    /// <summary>
    /// შეფუთვის ტიპი (ყუთი, პაკეტი, კოლოფი და ა.შ.)
    /// </summary>
    public string PackagingType { get; set; } = "Box"; // "Box", "Package", "Carton", "Pallet" და ა.შ.
    
    /// <summary>
    /// ერთ შეფუთვაში რამდენი ერთეული ჯდება 
    /// მაგ: 6 ბოთლი, 12 ბოთლი, 24 კოლა და ა.შ.
    /// </summary>
    public int UnitsPerPackage { get; set; }
    
    /// <summary>
    /// სრულად შეფუთული (დახურული) შეფუთვების რაოდენობა
    /// </summary>
    public int FullPackagesCount { get; set; }
    
    /// <summary>
    /// ნახევრად შეფუთული (დაწყებული/გახსნილი) შეფუთვების რაოდენობა
    /// </summary>
    public int PartialPackagesCount { get; set; }
    
    /// <summary>
    /// ნახევრად შეფუთულ შეფუთვებში არსებული ერთეულების რაოდენობა
    /// (ეს რიცხვი უნდა იყოს < UnitsPerPackage * PartialPackagesCount)
    /// </summary>
    public int UnitsInPartialPackages { get; set; }
    
    /// <summary>
    /// გამოთვლილი: სულ შეფუთვების რაოდენობა
    /// = FullPackagesCount + PartialPackagesCount
    /// </summary>
    public int TotalPackagesCount => FullPackagesCount + PartialPackagesCount;
    
    /// <summary>
    /// გამოთვლილი: სულ ერთეულების რაოდენობა (ყველა შეფუთვაში)
    /// = (FullPackagesCount * UnitsPerPackage) + UnitsInPartialPackages
    /// </summary>
    public int TotalUnitsCount => (FullPackagesCount * UnitsPerPackage) + UnitsInPartialPackages;
    
    /// <summary>
    /// შენიშვნები (მაგ: "წითელი ყუთები", "დაზიანებული შეფუთვა" და ა.შ.)
    /// </summary>
    public string? Notes { get; set; }
    
    // Navigation property
    public WarehouseStock WarehouseStock { get; set; } = null!;
}

/// <summary>
/// ალკოჰოლური პროდუქტების დამატებითი ინფორმაცია
/// ცალკე ცხრილი - მხოლოდ ალკოჰოლური პროდუქტებისთვის
/// One-to-One with WarehouseStock (optional)
/// </summary>
public class AlcoholicStockDetails : BaseEntity
{
    public Guid WarehouseStockId { get; set; } // FK to WarehouseStock
    
    /// <summary>
    /// პარტიის ნომერი (Batch Number)
    /// </summary>
    public string? BatchNumber { get; set; }
    
    /// <summary>
    /// აქციზის მარკის ნომერი
    /// </summary>
    public string? ExciseStampNumber { get; set; }
    
    /// <summary>
    /// სერტიფიკატის ნომერი
    /// </summary>
    public string? CertificateNumber { get; set; }
    
    /// <summary>
    /// ტემპერატურის რეჟიმი (°C)
    /// </summary>
    public decimal? StorageTemperature { get; set; }
    
    // Navigation property
    public WarehouseStock WarehouseStock { get; set; } = null!;
}
