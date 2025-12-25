using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// მწარმოებელი - ქარხანა
/// </summary>
public class Manufacturer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactInfo { get; set; }
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
}

