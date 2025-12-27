﻿using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Override GetByIdAsync to include related entities
    public new async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.UnitTypeRule)
            .Include(p => p.ProductDetails!)
                .ThenInclude(pd => pd.AlcoholicDetails!)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    // Override GetAllAsync to include related entities
    public new async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.UnitTypeRule)
            .Include(p => p.ProductDetails!)
                .ThenInclude(pd => pd.AlcoholicDetails!)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.CompanyProducts)
            .ThenInclude(cp => cp.Company)
            .Include(p => p.WarehouseStocks)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetProductsByCompanyAsync(Guid companyId)
    {
        return await _dbSet
            .Include(p => p.CompanyProducts)
            .Where(p => p.CompanyProducts.Any(cp => cp.CompanyId == companyId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _dbSet
            .Where(p => p.Name.Contains(searchTerm) || 
                       (p.Barcode != null && p.Barcode.Contains(searchTerm)))
            .ToListAsync();
    }
}

