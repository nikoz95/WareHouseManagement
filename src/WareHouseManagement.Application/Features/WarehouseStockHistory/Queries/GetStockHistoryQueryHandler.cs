using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.WarehouseStockHistory.Queries;

public class GetStockHistoryQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetStockHistoryQuery, Result<List<StockHistoryDto>>>
{
    public async Task<Result<List<StockHistoryDto>>> Handle(GetStockHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var histories = await unitOfWork.WarehouseStockHistories.GetHistoryAsync(
                request.WarehouseStockId,
                request.ProductId,
                request.OrderId,
                request.TransactionType,
                request.FromDate,
                request.ToDate);

            var historyDtos = histories.Select(h => new StockHistoryDto
            {
                Id = h.Id,
                WarehouseStockId = h.WarehouseStockId,
                ProductName = h.WarehouseStock.Product.Name,
                WarehouseName = h.WarehouseStock.WarehouseLocation.Warehouse.Name,
                LocationName = h.WarehouseStock.WarehouseLocation.LocationName,
                TransactionType = h.TransactionType,
                TransactionTypeName = GetTransactionTypeName(h.TransactionType),
                QuantityChange = h.QuantityChange,
                QuantityBefore = h.QuantityBefore,
                QuantityAfter = h.QuantityAfter,
                OrderId = h.OrderId,
                OrderNumber = h.Order?.OrderNumber,
                Reason = h.Reason,
                PerformedBy = h.PerformedBy,
                TransactionDate = h.TransactionDate
            }).ToList();

            return Result<List<StockHistoryDto>>.Success(historyDtos);
        }
        catch (Exception ex)
        {
            return Result<List<StockHistoryDto>>.Failure($"შეცდომა ისტორიის მიღებისას: {ex.Message}");
        }
    }

    private static string GetTransactionTypeName(Domain.Enums.StockTransactionType type)
    {
        return type switch
        {
            Domain.Enums.StockTransactionType.In => "შემოსვლა",
            Domain.Enums.StockTransactionType.Out => "გასვლა",
            Domain.Enums.StockTransactionType.Return => "დაბრუნება",
            Domain.Enums.StockTransactionType.Adjustment => "კორექტირება",
            Domain.Enums.StockTransactionType.Damage => "დაზიანება",
            Domain.Enums.StockTransactionType.Transfer => "გადატანა",
            _ => "უცნობი"
        };
    }
}

