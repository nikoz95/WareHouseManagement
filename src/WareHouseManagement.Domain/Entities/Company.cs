using WareHouseManagement.Domain.Common;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// კომპანია - პარტნიორი კომპანია რომელიც შეიძლება იყოს რესტორანი, ბარი, ქსელური კომპანია და ა.შ.
/// </summary>
public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public CompanyType CompanyType { get; set; }
    public bool IsPartner { get; set; } // პარტნიორი თუ არა
    
    // Navigation properties
    public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();
    public ICollection<CompanyLocation> CompanyLocations { get; set; } = new List<CompanyLocation>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

