using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IAlcoholicProductRepository : IGenericRepository<AlcoholicProduct>
{
    Task<AlcoholicProduct?> GetByProductIdAsync(Guid productId);
}
