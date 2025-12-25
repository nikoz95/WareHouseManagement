using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.WarehouseStocks.Queries;

public class GetAllWarehouseStocksQuery : IRequest<Result<List<WarehouseStockDto>>>
{
    public Guid? WarehouseLocationId { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? ManufacturerId { get; set; }
    public bool IncludePackagingDetails { get; set; } = false;
    public bool IncludeAlcoholicDetails { get; set; } = false;
}

