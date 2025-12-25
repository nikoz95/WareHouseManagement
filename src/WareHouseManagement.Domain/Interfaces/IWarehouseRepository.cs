﻿using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IWarehouseRepository : IGenericRepository<Warehouse>
{
    Task<Warehouse?> GetWarehouseWithStockAsync(Guid id);
    Task<IEnumerable<WarehouseStock>> GetStockByProductAsync(Guid productId);
    Task<IEnumerable<WarehouseStock>> GetStockByWarehouseLocationAsync(Guid warehouseLocationId);
    Task<WarehouseStock?> GetStockByProductAndLocationAsync(Guid productId, Guid warehouseLocationId);
    Task<WarehouseLocation?> GetLocationByIdAsync(Guid locationId);
    Task<List<WarehouseStock>> GetAllStocksAsync();
    Task<WarehouseStock?> GetStockByIdAsync(Guid stockId);
    Task AddStockAsync(WarehouseStock stock);
}

