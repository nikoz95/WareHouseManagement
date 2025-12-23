using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// საწყობის ლოკაცია - ობიექტი სადაც საწყობია
/// </summary>
public class WarehouseLocation : BaseEntity
{
    public Guid WarehouseId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Navigation properties
    public Warehouse Warehouse { get; set; } = null!;
    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
}

