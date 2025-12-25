using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IProductDetailsRepository : IGenericRepository<ProductDetails>
{
    Task<ProductDetails?> GetByProductIdAsync(Guid productId);
}

