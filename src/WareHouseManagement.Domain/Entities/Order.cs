using WareHouseManagement.Domain.Common;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// შეკვეთა
/// </summary>
public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public Guid? CompanyId { get; set; } // null თუ არაპარტნიორია
    public string CustomerName { get; set; } = string.Empty; // არაპარტნიორი კლიენტებისთვის
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DebtAmount { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public Company? Company { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

