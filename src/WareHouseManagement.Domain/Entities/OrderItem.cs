﻿﻿using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// შეკვეთის ელემენტი
/// </summary>
public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// რაოდენობა (პროდუქტის UnitTypeRule-ის მიხედვით)
    /// მაგ: თუ პროდუქტი არის "ლიტრებში", მაშინ ეს იქნება ლიტრების რაოდენობა
    /// </summary>
    public decimal Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    
    // Navigation properties
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

