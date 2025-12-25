using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// პროდუქტის ძირითადი დამატებითი ინფორმაცია (ყველა პროდუქტისთვის)
/// One-to-One relationship with Product
/// </summary>
public class ProductDetails : BaseEntity
{
    /// <summary>
    /// პროდუქტის ID (Foreign Key to Product)
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// წარმოშობის ქვეყანა
    /// </summary>
    public string? CountryOfOrigin { get; set; }
    
    /// <summary>
    /// პროდუქტის ტიპი/კატეგორია (მაგ: ღვინო, პივო, წყალი, წვენი და ა.შ.)
    /// </summary>
    public string? ProductType { get; set; }
    
    /// <summary>
    /// ვარგისიანობის ვადა (თვეებში)
    /// </summary>
    public int? ShelfLifeMonths { get; set; }
    
    /// <summary>
    /// დამატებითი შენიშვნები/აღწერა
    /// </summary>
    public string? AdditionalNotes { get; set; }
    
    // Navigation properties
    public Product Product { get; set; } = null!;
    
    /// <summary>
    /// ალკოჰოლური პროდუქტის დამატებითი დეტალები (optional)
    /// One-to-One relationship
    /// </summary>
    public AlcoholicProductDetails? AlcoholicDetails { get; set; }
}

/// <summary>
/// ალკოჰოლური პროდუქტის სპეციფიკური ინფორმაცია
/// ცალკე ცხრილი - მხოლოდ ალკოჰოლური პროდუქტებისთვის
/// One-to-One with ProductDetails (optional)
/// </summary>
public class AlcoholicProductDetails : BaseEntity
{
    public Guid ProductDetailsId { get; set; } // FK to ProductDetails
    
    /// <summary>
    /// ალკოჰოლის პროცენტი
    /// </summary>
    public decimal AlcoholPercentage { get; set; }
    
    /// <summary>
    /// რეგიონი/საწარმო (მაგ: კახეთი, რაჭა და ა.შ.)
    /// </summary>
    public string? Region { get; set; }
    
    /// <summary>
    /// რეკომენდებული მიწოდების ტემპერატურა (°C)
    /// </summary>
    public decimal? ServingTemperature { get; set; }
    
    /// <summary>
    /// ალკოჰოლური სასმლის კლასი/ხარისხი (მაგ: Premium, Standard და ა.შ.)
    /// </summary>
    public string? QualityClass { get; set; }
    
    // Navigation property
    public ProductDetails ProductDetails { get; set; } = null!;
}
