﻿﻿namespace WareHouseManagement.Application.DTOs;

public class WarehouseStockDto
{
    public Guid Id { get; set; }
    public Guid WarehouseLocationId { get; set; }
    public string WarehouseLocationName { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; } = string.Empty;
    public int BottlesPerBox { get; set; }
    public int QuantityInBottles { get; set; }
    public int QuantityInBoxes { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateWarehouseStockDto
{
    public Guid WarehouseLocationId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ManufacturerId { get; set; }
    public int QuantityInBottles { get; set; }
    public int QuantityInBoxes { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

public class UpdateWarehouseStockDto
{
    public Guid Id { get; set; }
    public int BottlesPerBox { get; set; }
    public int QuantityInBottles { get; set; }
    public int QuantityInBoxes { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

