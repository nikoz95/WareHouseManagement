using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class ProductDetailsRepository : GenericRepository<ProductDetails>, IProductDetailsRepository
{
    public ProductDetailsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ProductDetails?> GetByProductIdAsync(Guid productId)
    {
        return await _context.ProductDetails
            .FirstOrDefaultAsync(pd => pd.ProductId == productId && !pd.IsDeleted);
    }
}

