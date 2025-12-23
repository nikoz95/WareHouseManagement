using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class ManufacturerRepository : GenericRepository<Manufacturer>, IManufacturerRepository
{
    public ManufacturerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Manufacturer>> SearchManufacturersAsync(string searchTerm)
    {
        return await _dbSet
            .Where(m => m.Name.Contains(searchTerm) || 
                       (m.Country != null && m.Country.Contains(searchTerm)))
            .ToListAsync();
    }
}

