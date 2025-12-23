using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<Result<ProductDto>>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public int BottlesPerBox { get; set; } = 6;
    public decimal Volume { get; set; }
    public decimal AlcoholPercentage { get; set; }
}
