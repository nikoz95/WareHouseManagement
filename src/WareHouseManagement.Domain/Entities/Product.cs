using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// ალკოჰოლური პროდუქტი
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int BottlesPerBox { get; set; } = 6; // ყუთში ბოთლების რაოდენობა
    public decimal Volume { get; set; } // მოცულობა (მაგ: 0.5 ლიტრი)
    public decimal AlcoholPercentage { get; set; } // ალკოჰოლის პროცენტი
    
    // Navigation properties
    public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();
    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

