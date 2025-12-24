using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Domain.Interfaces;

public interface IUnitTypeRuleRepository : IGenericRepository<UnitTypeRule>
{
    Task<UnitTypeRule?> GetByUnitTypeAsync(UnitType unitType);
    Task<IEnumerable<UnitTypeRule>> GetActiveRulesAsync();
}

