namespace WareHouseManagement.Infrastructure.Services;

/// <summary>
/// Excel Import/Export მოდელები
/// </summary>
public class ProductImportDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    // Alcoholic Details
    public decimal? AlcoholPercentage { get; set; }
    public decimal? Volume { get; set; }
    public string? Color { get; set; }
    public decimal? SugarContent { get; set; }
    public string? Manufacturer { get; set; }
    public string? Region { get; set; }
    public string? GrapeVariety { get; set; }
    public string? ServingTemperature { get; set; }
    public string? QualityClass { get; set; }
}

public class CompanyImportDto
{
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string CompanyType { get; set; } = "Supplier"; // Supplier, Customer, Both
    public bool IsPartner { get; set; }
}

public class ManufacturerImportDto
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Region { get; set; }
}

public class WarehouseImportDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
}

public class WarehouseStockImportDto
{
    public string WarehouseName { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string ProductBarcode { get; set; } = string.Empty;
    public string ManufacturerName { get; set; } = string.Empty;
    public int TotalUnits { get; set; }
    public int UnitsPerPackage { get; set; } = 12;
}

