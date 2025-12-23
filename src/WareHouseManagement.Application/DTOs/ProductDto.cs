namespace WareHouseManagement.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int BottlesPerBox { get; set; }
    public decimal Volume { get; set; }
    public decimal AlcoholPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int BottlesPerBox { get; set; } = 6;
    public decimal Volume { get; set; }
    public decimal AlcoholPercentage { get; set; }
}

public class UpdateProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int BottlesPerBox { get; set; }
    public decimal Volume { get; set; }
    public decimal AlcoholPercentage { get; set; }
}

