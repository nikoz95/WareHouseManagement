using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IAlcoholicProductDetailsRepository : IGenericRepository<AlcoholicProductDetails>
{
    Task<AlcoholicProductDetails?> GetByProductDetailsIdAsync(Guid productDetailsId);
}
