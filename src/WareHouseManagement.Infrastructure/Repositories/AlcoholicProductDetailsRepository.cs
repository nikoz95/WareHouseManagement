using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class AlcoholicProductDetailsRepository : GenericRepository<AlcoholicProductDetails>, IAlcoholicProductDetailsRepository
{
    public AlcoholicProductDetailsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<AlcoholicProductDetails?> GetByProductDetailsIdAsync(Guid productDetailsId)
    {
        return await _context.AlcoholicProductDetails
            .FirstOrDefaultAsync(apd => apd.ProductDetailsId == productDetailsId && !apd.IsDeleted);
    }
}

