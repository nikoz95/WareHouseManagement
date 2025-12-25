using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Application.Features.WarehouseStockHistory.Queries;

public class GetStockHistoryQuery : IRequest<Result<List<StockHistoryDto>>>
{
    public Guid? WarehouseStockId { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? OrderId { get; set; }
    public StockTransactionType? TransactionType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
