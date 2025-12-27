﻿﻿using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Override GetByIdAsync to include WarehouseLocations
    public new async Task<Warehouse?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(w => w.WarehouseLocations)
            .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
    }

    // Override GetAllAsync to include WarehouseLocations
    public new async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        return await _dbSet
            .Include(w => w.WarehouseLocations)
            .Where(w => !w.IsDeleted)
            .ToListAsync();
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

    public async Task<List<WarehouseStock>> GetAllStocksAsync(bool includePackaging = true, bool includeAlcoholic = true)
    {
        var query = _context.WarehouseStocks
            .Include(ws => ws.WarehouseLocation)
                .ThenInclude(wl => wl.Warehouse)
            .Include(ws => ws.Product)
                .ThenInclude(p => p.UnitTypeRule)
            .Include(ws => ws.Manufacturer)
            .AsQueryable();

        // კონდიციურად ვატვირთავთ PackagingDetails-ს
        if (includePackaging)
        {
            query = query.Include(ws => ws.PackagingDetails);
        }

        // კონდიციურად ვატვირთავთ AlcoholicDetails-ს
        if (includeAlcoholic)
        {
            query = query.Include(ws => ws.AlcoholicDetails);
        }

        return await query
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
