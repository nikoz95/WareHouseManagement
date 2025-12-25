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
    
    // დამატებითი დეტალური ინფორმაცია (optional - ნებისმიერი პროდუქტისთვის)
    public string? CountryOfOrigin { get; set; }
    public string? ProductType { get; set; }
    public int? ShelfLifeMonths { get; set; }
    public string? AdditionalNotes { get; set; }
    
    // ალკოჰოლური პროდუქტის სპეციფიკური ინფორმაცია (optional)
    public decimal? AlcoholPercentage { get; set; }
    public string? Region { get; set; }
    public decimal? ServingTemperature { get; set; }
    public string? QualityClass { get; set; }
}
