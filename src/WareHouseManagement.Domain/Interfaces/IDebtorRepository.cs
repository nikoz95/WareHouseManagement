﻿using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IDebtorRepository : IGenericRepository<Debtor>
{
    Task<IEnumerable<Debtor>> GetDebtorsWithOutstandingDebtAsync();
    Task<Debtor?> GetDebtorByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<Debtor>> GetDebtorsByCompanyAsync(Guid companyId);
    Task<decimal> GetTotalOutstandingDebtAsync();
}

