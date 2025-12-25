using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface ICompanyRepository : IGenericRepository<Company>
{
    Task<IEnumerable<Company>> GetPartnerCompaniesAsync();
    Task<IEnumerable<Company>> GetCompaniesByTypeAsync(Enums.CompanyType type);
    Task<Company?> GetCompanyWithProductsAsync(Guid id);
    Task<Company?> GetCompanyWithLocationsAsync(Guid id);
    Task<IEnumerable<Company>> GetAllCompaniesWithLocationsAsync();
}

