﻿﻿namespace WareHouseManagement.Application.DTOs;

public class WarehouseStockDto
{
    public Guid Id { get; set; }
    public Guid WarehouseLocationId { get; set; }
    public string WarehouseLocationName { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid? ManufacturerId { get; set; } // nullable - შეიძლება იყოს უცნობი მწარმოებელი
    public string? ManufacturerName { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string StockType { get; set; } = string.Empty; // "Alcoholic" or "General"
    
    // შეფუთვის დეტალები (უნივერსალური - ნებისმიერი პროდუქტისთვის)
    public string? PackagingType { get; set; } // "Box", "Package", "Carton" და ა.შ.
    public int? UnitsPerPackage { get; set; }
    public int? FullPackagesCount { get; set; }
    public int? PartialPackagesCount { get; set; }
    public int? UnitsInPartialPackages { get; set; }
    public int? TotalPackagesCount { get; set; }
    public int? TotalUnitsCount { get; set; }
    public string? PackagingNotes { get; set; }
    
    // ალკოჰოლური პროდუქტის დამატებითი ველები (თუ StockType == "Alcoholic")
    public string? BatchNumber { get; set; }
    public string? ExciseStampNumber { get; set; }
    public string? CertificateNumber { get; set; }
    public decimal? StorageTemperature { get; set; }
    
    // ზოგადი პროდუქტის დამატებითი ველები (თუ StockType == "General")
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public class CreateWarehouseStockDto
{
    public Guid WarehouseLocationId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ManufacturerId { get; set; } // nullable - შეიძლება იყოს უცნობი მწარმოებელი
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
    
    // შეფუთვის დეტალები (optional - ნებისმიერი პროდუქტისთვის)
    public string? PackagingType { get; set; } // "Box", "Package", "Carton" და ა.შ.
    public int? UnitsPerPackage { get; set; }
    public int? FullPackagesCount { get; set; }
    public int? PartialPackagesCount { get; set; }
    public int? UnitsInPartialPackages { get; set; }
    public string? PackagingNotes { get; set; }
    
    // ალკოჰოლური პროდუქტის დამატებითი ველები (optional)
    public string? BatchNumber { get; set; }
    public string? ExciseStampNumber { get; set; }
    public string? CertificateNumber { get; set; }
    public decimal? StorageTemperature { get; set; }
    
    // ზოგადი პროდუქტის შენიშვნები (optional)
    public string? Notes { get; set; }
}

public class UpdateWarehouseStockDto
{
    public Guid Id { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
    
    // შეფუთვის დეტალები (optional)
    public string? PackagingType { get; set; }
    public int? UnitsPerPackage { get; set; }
    public int? FullPackagesCount { get; set; }
    public int? PartialPackagesCount { get; set; }
    public int? UnitsInPartialPackages { get; set; }
    public string? PackagingNotes { get; set; }
    
    // ალკოჰოლური პროდუქტის დამატებითი ველები (optional)
    public string? BatchNumber { get; set; }
    public string? ExciseStampNumber { get; set; }
    public string? CertificateNumber { get; set; }
    public decimal? StorageTemperature { get; set; }
    
    // ზოგადი პროდუქტის შენიშვნები (optional)
    public string? Notes { get; set; }
}

