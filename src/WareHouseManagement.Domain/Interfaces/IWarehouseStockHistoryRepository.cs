using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Domain.Interfaces;

public interface IWarehouseStockHistoryRepository : IGenericRepository<WarehouseStockHistory>
{
    Task<List<WarehouseStockHistory>> GetHistoryAsync(
        Guid? warehouseStockId = null,
        Guid? productId = null,
        Guid? orderId = null,
        StockTransactionType? transactionType = null,
        DateTime? fromDate = null,
        DateTime? toDate = null);
}

