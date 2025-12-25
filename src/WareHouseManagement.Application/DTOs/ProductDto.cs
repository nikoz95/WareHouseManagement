namespace WareHouseManagement.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public Guid UnitTypeRuleId { get; set; }
    public string UnitTypeName { get; set; } = string.Empty; // ქართული სახელი
    public string UnitTypeAbbreviation { get; set; } = string.Empty; // შეკვეცილი ფორმა
    
    // დამატებითი დეტალური ინფორმაცია (თუ ProductDetails არსებობს)
    public bool HasDetails { get; set; }
    public string? CountryOfOrigin { get; set; }
    public string? ProductType { get; set; }
    public int? ShelfLifeMonths { get; set; }
    public string? AdditionalNotes { get; set; }
    
    // ალკოჰოლური პროდუქტის დამატებითი ინფორმაცია (თუ AlcoholicDetails არსებობს)
    public bool IsAlcoholic { get; set; }
    public decimal? AlcoholPercentage { get; set; }
    public string? Region { get; set; }
    public decimal? ServingTemperature { get; set; }
    public string? QualityClass { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public Guid UnitTypeRuleId { get; set; }
    
    // დამატებითი დეტალური ინფორმაცია (optional - ნებისმიერი პროდუქტისთვის)
    public decimal? AlcoholPercentage { get; set; }
    public string? CountryOfOrigin { get; set; }
    public string? ProductType { get; set; } // ღვინო, პივო, წყალი, წვენი და ა.შ.
    public int? ShelfLifeMonths { get; set; }
    public string? AdditionalNotes { get; set; }
}

public class UpdateProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public Guid UnitTypeRuleId { get; set; }
    
    // დამატებითი დეტალური ინფორმაცია (optional)
    public string? CountryOfOrigin { get; set; }
    public string? ProductType { get; set; }
    public int? ShelfLifeMonths { get; set; }
    public string? AdditionalNotes { get; set; }
    
    // ალკოჰოლური პროდუქტის ინფორმაცია (optional)
    public decimal? AlcoholPercentage { get; set; }
    public string? Region { get; set; }
    public decimal? ServingTemperature { get; set; }
    public string? QualityClass { get; set; }
}

