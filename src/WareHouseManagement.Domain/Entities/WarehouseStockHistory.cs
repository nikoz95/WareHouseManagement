using WareHouseManagement.Domain.Common;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// საწყობის მარაგის ისტორია - ყველა ტრანზაქციის აღრიცხვა
/// </summary>
public class WarehouseStockHistory : BaseEntity
{
    public Guid WarehouseStockId { get; set; }
    
    /// <summary>
    /// ტრანზაქციის ტიპი (შემოსვლა, გასვლა, კორექტირება)
    /// </summary>
    public StockTransactionType TransactionType { get; set; }
    
    /// <summary>
    /// რაოდენობა რომელიც შეიცვალა (დადებითი შემოსვლისას, უარყოფითი გასვლისას)
    /// </summary>
    public decimal QuantityChange { get; set; }
    
    /// <summary>
    /// რაოდენობა ამ ტრანზაქციამდე
    /// </summary>
    public decimal QuantityBefore { get; set; }
    
    /// <summary>
    /// რაოდენობა ამ ტრანზაქციის შემდეგ
    /// </summary>
    public decimal QuantityAfter { get; set; }
    
    /// <summary>
    /// დაკავშირებული შეკვეთის ID (თუ ტრანზაქცია შეკვეთიდან არის)
    /// </summary>
    public Guid? OrderId { get; set; }
    
    /// <summary>
    /// ტრანზაქციის მიზეზი/აღწერა
    /// </summary>
    public string? Reason { get; set; }
    
    /// <summary>
    /// ვინ განახორციელა ტრანზაქცია (მომხმარებლის ID)
    /// </summary>
    public string? PerformedBy { get; set; }
    
    /// <summary>
    /// ტრანზაქციის თარიღი
    /// </summary>
    public DateTime TransactionDate { get; set; }
    
    // Navigation properties
    public WarehouseStock WarehouseStock { get; set; } = null!;
    public Order? Order { get; set; }
}

