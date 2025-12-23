using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Warehouse?> GetWarehouseWithStockAsync(Guid id)
    {
        return await _dbSet
            .Include(w => w.WarehouseLocations)
            .ThenInclude(wl => wl.WarehouseStocks)
            .ThenInclude(ws => ws.Product)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<WarehouseStock>> GetStockByProductAsync(Guid productId)
    {
        return await _context.WarehouseStocks
            .Include(ws => ws.WarehouseLocation)
            .ThenInclude(wl => wl.Warehouse)
            .Include(ws => ws.Product)
            .Include(ws => ws.Manufacturer)
            .Where(ws => ws.ProductId == productId)
            .ToListAsync();
    }

    public async Task<IEnumerable<WarehouseStock>> GetStockByWarehouseLocationAsync(Guid warehouseLocationId)
    {
        return await _context.WarehouseStocks
            .Include(ws => ws.Product)
            .Include(ws => ws.Manufacturer)
            .Where(ws => ws.WarehouseLocationId == warehouseLocationId)
            .ToListAsync();
    }

    public async Task<WarehouseStock?> GetStockByProductAndLocationAsync(Guid productId, Guid warehouseLocationId)
    {
        return await _context.WarehouseStocks
            .Include(ws => ws.Product)
            .Include(ws => ws.Manufacturer)
            .FirstOrDefaultAsync(ws => ws.ProductId == productId && ws.WarehouseLocationId == warehouseLocationId);
    }
}

