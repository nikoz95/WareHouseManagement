using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class DebtorRepository : GenericRepository<Debtor>, IDebtorRepository
{
    public DebtorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Debtor>> GetDebtorsWithOutstandingDebtAsync()
    {
        return await _dbSet
            .Include(d => d.Company)
            .Where(d => d.RemainingDebt > 0)
            .OrderByDescending(d => d.RemainingDebt)
            .ToListAsync();
    }

    public async Task<Debtor?> GetDebtorByCompanyIdAsync(Guid companyId)
    {
        return await _dbSet
            .Include(d => d.Company)
            .FirstOrDefaultAsync(d => d.CompanyId == companyId);
    }

    public async Task<decimal> GetTotalOutstandingDebtAsync()
    {
        return await _dbSet.SumAsync(d => d.RemainingDebt);
    }
}

