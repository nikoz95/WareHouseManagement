using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<Result<ProductDto>>
{
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
