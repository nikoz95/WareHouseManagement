using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// საწყობი
/// </summary>
public class Warehouse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Phone { get; set; }
    
    // Navigation properties
    public ICollection<WarehouseLocation> WarehouseLocations { get; set; } = new List<WarehouseLocation>();
}

