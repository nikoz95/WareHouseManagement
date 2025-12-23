using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// საწყობის მარაგი - პროდუქტის რაოდენობა საწყობში
/// </summary>
public class WarehouseStock : BaseEntity
{
    public Guid WarehouseLocationId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ManufacturerId { get; set; }
    public int QuantityInBottles { get; set; } // რაოდენობა ბოთლებში
    public int QuantityInBoxes { get; set; } // რაოდენობა ყუთებში
    public decimal PurchasePrice { get; set; } // შესყიდვის ფასი
    public DateTime? ExpirationDate { get; set; }
    
    // Navigation properties
    public WarehouseLocation WarehouseLocation { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public Manufacturer Manufacturer { get; set; } = null!;
}

