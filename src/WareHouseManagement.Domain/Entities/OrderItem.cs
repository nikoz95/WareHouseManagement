﻿using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// შეკვეთის ელემენტი
/// </summary>
public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int BottlesPerBox { get; set; } = 6; // ყუთში ბოთლების რაოდენობა (შენახულია შეკვეთის დროს)
    public int QuantityInBottles { get; set; }
    public int QuantityInBoxes { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    
    // Navigation properties
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

