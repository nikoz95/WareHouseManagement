﻿using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// პროდუქტი (ზოგადი)
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; } // ფასი
    
    /// <summary>
    /// საზომი ერთეულის წესის ID (მიმართება UnitTypeRule-ზე)
    /// </summary>
    public Guid UnitTypeRuleId { get; set; }
    
    // Navigation properties
    public UnitTypeRule UnitTypeRule { get; set; } = null!;
    public AlcoholicProduct? AlcoholicProduct { get; set; } // თუ პროდუქტი ალკოჰოლურია
    public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();
    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

