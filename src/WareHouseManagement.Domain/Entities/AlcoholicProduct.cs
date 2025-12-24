using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// ალკოჰოლური პროდუქტის დამატებითი ინფორმაცია
/// One-to-One relationship with Product
/// </summary>
public class AlcoholicProduct : BaseEntity
{
    /// <summary>
    /// პროდუქტის ID (Foreign Key to Product)
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// ალკოჰოლის პროცენტი
    /// </summary>
    public decimal AlcoholPercentage { get; set; }
    
    /// <summary>
    /// მწარმოებლის ქვეყანა
    /// </summary>
    public string? CountryOfOrigin { get; set; }
    
    /// <summary>
    /// ტიპი (ღვინო, პივო, არაყი და ა.შ.)
    /// </summary>
    public string? AlcoholType { get; set; }
    
    /// <summary>
    /// ვარგისიანობის ვადა (თვეებში)
    /// </summary>
    public int? ShelfLifeMonths { get; set; }
    
    // Navigation property
    public Product Product { get; set; } = null!;
}

