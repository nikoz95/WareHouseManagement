using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetProductWithDetailsAsync(Guid id);
    Task<IEnumerable<Product>> GetProductsByCompanyAsync(Guid companyId);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
}

