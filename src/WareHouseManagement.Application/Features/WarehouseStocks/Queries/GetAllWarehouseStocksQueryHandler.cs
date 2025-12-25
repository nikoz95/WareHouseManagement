using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.WarehouseStocks.Queries;

public class GetAllWarehouseStocksQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    : IRequestHandler<GetAllWarehouseStocksQuery, Result<List<WarehouseStockDto>>>
{
    public async Task<Result<List<WarehouseStockDto>>> Handle(GetAllWarehouseStocksQuery request, CancellationToken cancellationToken)
    {
        try
        {
          
            // For now, get all stocks
            var stocks = await unitOfWork.Warehouses.GetAllStocksAsync();

            // თუ გვინდა მხოლოდ კონკრეტული ლოკაციის stock-ები
            if (request.WarehouseLocationId.HasValue)
            {
                stocks = stocks.Where(s => s.WarehouseLocationId == request.WarehouseLocationId.Value).ToList();
            }

            // თუ გვინდა მხოლოდ კონკრეტული პროდუქტის stock-ები
            if (request.ProductId.HasValue)
            {
                stocks = stocks.Where(s => s.ProductId == request.ProductId.Value).ToList();
            }

            // თუ გვინდა მხოლოდ კონკრეტული მწარმოებლის stock-ები
            if (request.ManufacturerId.HasValue)
            {
                stocks = stocks.Where(s => s.ManufacturerId == request.ManufacturerId.Value).ToList();
            }

            var stockDtos = stocks.Select(mapper.MapToWarehouseStockDto).ToList();
            
            return Result<List<WarehouseStockDto>>.Success(stockDtos);
        }
        catch (Exception ex)
        {
            return Result<List<WarehouseStockDto>>.Failure($"შეცდომა საწყობის მარაგების მიღებისას: {ex.Message}");
        }
    }
}

