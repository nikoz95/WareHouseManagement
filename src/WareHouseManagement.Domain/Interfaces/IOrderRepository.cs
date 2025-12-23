using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetOrderWithItemsAsync(Guid id);
    Task<IEnumerable<Order>> GetOrdersByCompanyAsync(Guid companyId);
    Task<IEnumerable<Order>> GetPendingOrdersAsync();
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<string> GenerateOrderNumberAsync();
}

