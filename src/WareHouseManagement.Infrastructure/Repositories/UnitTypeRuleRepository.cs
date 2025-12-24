using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class UnitTypeRuleRepository : GenericRepository<UnitTypeRule>, IUnitTypeRuleRepository
{
    public UnitTypeRuleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UnitTypeRule?> GetByUnitTypeAsync(UnitType unitType)
    {
        return await _context.UnitTypeRules
            .FirstOrDefaultAsync(r => r.UnitType == unitType && !r.IsDeleted);
    }

    public async Task<IEnumerable<UnitTypeRule>> GetActiveRulesAsync()
    {
        return await _context.UnitTypeRules
            .Where(r => r.IsActive && !r.IsDeleted)
            .OrderBy(r => r.UnitType)
            .ToListAsync();
    }
}

