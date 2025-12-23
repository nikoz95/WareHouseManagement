using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetOrderWithItemsAsync(Guid id)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Company)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCompanyAsync(Guid companyId)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.CompanyId == companyId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Include(o => o.Company)
            .Where(o => o.Status == Domain.Enums.OrderStatus.Pending)
            .OrderBy(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Include(o => o.Company)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        var lastOrder = await _dbSet
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        var orderCount = await _dbSet.CountAsync();
        var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{(orderCount + 1):D6}";

        return orderNumber;
    }
}

