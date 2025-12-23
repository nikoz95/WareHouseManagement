using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// კომპანიის ლოკაცია - ობიექტები სადაც კომპანია მოქმედებს
/// </summary>
public class CompanyLocation : BaseEntity
{
    public Guid CompanyId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Phone { get; set; }
    
    // Navigation properties
    public Company Company { get; set; } = null!;
}

