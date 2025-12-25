using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Application.DTOs;

public class StockHistoryDto
{
    public Guid Id { get; set; }
    public Guid WarehouseStockId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public StockTransactionType TransactionType { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;
    public decimal QuantityChange { get; set; }
    public decimal QuantityBefore { get; set; }
    public decimal QuantityAfter { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public string? Reason { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime TransactionDate { get; set; }
}

