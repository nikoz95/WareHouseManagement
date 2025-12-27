﻿using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Override GetByIdAsync to include CompanyLocations
    public new async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.CompanyLocations.Where(cl => !cl.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    // Override GetAllAsync to include CompanyLocations
    public new async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.CompanyLocations.Where(cl => !cl.IsDeleted))
            .Where(c => !c.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetPartnerCompaniesAsync()
    {
        return await _dbSet.Where(c => c.IsPartner).ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetCompaniesByTypeAsync(CompanyType type)
    {
        return await _dbSet.Where(c => c.CompanyType == type).ToListAsync();
    }

    public async Task<Company?> GetCompanyWithProductsAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.CompanyProducts)
            .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Company?> GetCompanyWithLocationsAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.CompanyLocations.Where(cl => !cl.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesWithLocationsAsync()
    {
        return await _dbSet
            .Include(c => c.CompanyLocations.Where(cl => !cl.IsDeleted))
            .ToListAsync();
    }
}

