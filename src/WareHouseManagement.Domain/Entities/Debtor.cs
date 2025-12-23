using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// დებიტორი - კომპანია რომელსაც აქვს დავალიანება
/// </summary>
public class Debtor : BaseEntity
{
    public Guid? CompanyId { get; set; } // null თუ არაპარტნიორია
    public string DebtorName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public decimal TotalDebt { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingDebt { get; set; }
    public DateTime DebtDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public Company? Company { get; set; }
}

