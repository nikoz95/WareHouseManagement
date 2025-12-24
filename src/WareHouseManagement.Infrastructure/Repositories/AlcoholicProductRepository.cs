using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class AlcoholicProductRepository : GenericRepository<AlcoholicProduct>, IAlcoholicProductRepository
{
    public AlcoholicProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<AlcoholicProduct?> GetByProductIdAsync(Guid productId)
    {
        return await _context.AlcoholicProducts
            .FirstOrDefaultAsync(ap => ap.ProductId == productId && !ap.IsDeleted);
    }
}

