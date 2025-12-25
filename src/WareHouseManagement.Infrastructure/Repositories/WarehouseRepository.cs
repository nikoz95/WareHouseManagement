﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<WarehouseLocation?> GetLocationByIdAsync(Guid locationId)
    {
        return await _context.Set<WarehouseLocation>()
            .Include(wl => wl.Warehouse)
            .FirstOrDefaultAsync(wl => wl.Id == locationId && !wl.IsDeleted);
    }

    public async Task<List<WarehouseStock>> GetAllStocksAsync()
    {
        return await _context.WarehouseStocks
            .Include(ws => ws.WarehouseLocation)
                .ThenInclude(wl => wl.Warehouse)
            .Include(ws => ws.Product)
                .ThenInclude(p => p.UnitTypeRule)
            .Include(ws => ws.Manufacturer)
            .Where(ws => !ws.IsDeleted)
            .ToListAsync();
    }

    public async Task<WarehouseStock?> GetStockByIdAsync(Guid stockId)
    {
        return await _context.WarehouseStocks
            .Include(ws => ws.WarehouseLocation)
                .ThenInclude(wl => wl.Warehouse)
            .Include(ws => ws.Product)
                .ThenInclude(p => p.UnitTypeRule)
            .Include(ws => ws.Manufacturer)
            .Include(ws => ws.PackagingDetails)
            .Include(ws => ws.AlcoholicDetails)
            .FirstOrDefaultAsync(ws => ws.Id == stockId && !ws.IsDeleted);
    }

    public async Task AddStockAsync(WarehouseStock stock)
    {
        await _context.WarehouseStocks.AddAsync(stock);
    }
}
