using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IManufacturerRepository : IGenericRepository<Manufacturer>
{
    Task<IEnumerable<Manufacturer>> SearchManufacturersAsync(string searchTerm);
}

