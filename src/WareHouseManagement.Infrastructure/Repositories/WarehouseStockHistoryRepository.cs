using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class WarehouseStockHistoryRepository : GenericRepository<WarehouseStockHistory>, IWarehouseStockHistoryRepository
{
    public WarehouseStockHistoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<WarehouseStockHistory>> GetHistoryAsync(
        Guid? warehouseStockId = null,
        Guid? productId = null,
        Guid? orderId = null,
        StockTransactionType? transactionType = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var query = _context.WarehouseStockHistories
            .Include(h => h.WarehouseStock)
                .ThenInclude(ws => ws.Product)
            .Include(h => h.WarehouseStock)
                .ThenInclude(ws => ws.WarehouseLocation)
                .ThenInclude(wl => wl.Warehouse)
            .Include(h => h.Order)
            .AsQueryable();

        if (warehouseStockId.HasValue)
        {
            query = query.Where(h => h.WarehouseStockId == warehouseStockId.Value);
        }

        if (productId.HasValue)
        {
            query = query.Where(h => h.WarehouseStock.ProductId == productId.Value);
        }

        if (orderId.HasValue)
        {
            query = query.Where(h => h.OrderId == orderId.Value);
        }

        if (transactionType.HasValue)
        {
            query = query.Where(h => h.TransactionType == transactionType.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(h => h.TransactionDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(h => h.TransactionDate <= toDate.Value);
        }

        return await query
            .OrderByDescending(h => h.TransactionDate)
            .ToListAsync();
    }
}

