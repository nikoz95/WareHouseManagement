using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// კომპანია-პროდუქტი კავშირი - რა ფასით შეუძენია პროდუქტი კომპანიას
/// </summary>
public class CompanyProduct : BaseEntity
{
    public Guid CompanyId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? CompanyLocationId { get; set; }
    public decimal CommercialPrice { get; set; } // კომერციული ფასი რა ფასით ყიდულობს კომპანია
    
    // Navigation properties
    public Company Company { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public CompanyLocation? CompanyLocation { get; set; }
}

