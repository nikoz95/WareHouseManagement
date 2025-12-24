namespace WareHouseManagement.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal AlcoholPercentage { get; set; }
    public Guid UnitTypeRuleId { get; set; }
    public string UnitTypeName { get; set; } = string.Empty; // ქართული სახელი
    public string UnitTypeAbbreviation { get; set; } = string.Empty; // შეკვეცილი ფორმა
    public decimal? UnitValue { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal AlcoholPercentage { get; set; }
    public Guid UnitTypeRuleId { get; set; }
    public decimal? UnitValue { get; set; }
}

public class UpdateProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public Guid UnitTypeRuleId { get; set; }
    
    // ალკოჰოლური პროდუქტის ინფორმაცია (optional)
    public bool IsAlcoholic { get; set; }
    public decimal? AlcoholPercentage { get; set; }
    public string? AlcoholType { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int? ShelfLifeMonths { get; set; }
}

