﻿using Riok.Mapperly.Abstractions;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Application.Mappings;

[Mapper]
public partial class ApplicationMapper
{
    // Company mappings - manual for CompanyTypeDescription
    public CompanyDto MapToCompanyDto(Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            TaxId = company.TaxId,
            Address = company.Address,
            Phone = company.Phone,
            Email = company.Email,
            CompanyType = company.CompanyType,
            CompanyTypeDescription = company.CompanyType.ToString(),
            IsPartner = company.IsPartner,
            CreatedAt = company.CreatedAt
        };
    }
    
    [MapperIgnoreTarget(nameof(Company.CompanyProducts))]
    [MapperIgnoreTarget(nameof(Company.CompanyLocations))]
    [MapperIgnoreTarget(nameof(Company.Orders))]
    [MapperIgnoreTarget(nameof(Company.Id))]
    [MapperIgnoreTarget(nameof(Company.CreatedAt))]
    [MapperIgnoreTarget(nameof(Company.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Company.IsDeleted))]
    public partial Company MapToCompany(CreateCompanyDto dto);
    
    [MapperIgnoreTarget(nameof(Company.CompanyProducts))]
    [MapperIgnoreTarget(nameof(Company.CompanyLocations))]
    [MapperIgnoreTarget(nameof(Company.Orders))]
    [MapperIgnoreTarget(nameof(Company.CreatedAt))]
    [MapperIgnoreTarget(nameof(Company.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Company.IsDeleted))]
    public partial Company MapToCompany(UpdateCompanyDto dto);

    // Product mappings - manual mapping needed for complex nested properties
    public ProductDto MapToProductDto(Product product)
    {
        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Barcode = product.Barcode,
            Price = product.Price,
            UnitTypeRuleId = product.UnitTypeRuleId,
            UnitTypeName = product.UnitTypeRule?.NameKa ?? "",
            UnitTypeAbbreviation = product.UnitTypeRule?.Abbreviation ?? "",
            IsAlcoholic = product.AlcoholicProduct != null,
            AlcoholPercentage = product.AlcoholicProduct?.AlcoholPercentage,
            AlcoholType = product.AlcoholicProduct?.AlcoholType,
            CountryOfOrigin = product.AlcoholicProduct?.CountryOfOrigin,
            CreatedAt = product.CreatedAt
        };
        return dto;
    }

    // Order mappings - manual mapping for nested properties
    public OrderDto MapToOrderDto(Order order)
    {
        var dto = new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CompanyId = order.CompanyId,
            CompanyName = order.Company?.Name,
            CustomerName = order.CustomerName,
            CustomerPhone = order.CustomerPhone,
            CustomerEmail = order.CustomerEmail,
            OrderDate = order.OrderDate,
            Status = order.Status,
            StatusDescription = order.Status.ToString(),
            PaymentStatus = order.PaymentStatus,
            PaymentStatusDescription = order.PaymentStatus.ToString(),
            TotalAmount = order.TotalAmount,
            PaidAmount = order.PaidAmount,
            DebtAmount = order.DebtAmount,
            Notes = order.Notes,
            OrderItems = order.OrderItems?.Select(MapToOrderItemDto).ToList() ?? new List<OrderItemDto>()
        };
        return dto;
    }
    
    public OrderItemDto MapToOrderItemDto(OrderItem orderItem)
    {
        return new OrderItemDto
        {
            Id = orderItem.Id,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.Product?.Name ?? "",
            QuantityInBottles = orderItem.QuantityInBottles,
            QuantityInBoxes = orderItem.QuantityInBoxes,
            UnitPrice = orderItem.UnitPrice,
            TotalPrice = orderItem.TotalPrice
        };
    }

    // WarehouseStock mappings
    public WarehouseStockDto MapToWarehouseStockDto(WarehouseStock warehouseStock)
    {
        return new WarehouseStockDto
        {
            Id = warehouseStock.Id,
            WarehouseLocationId = warehouseStock.WarehouseLocationId,
            WarehouseLocationName = warehouseStock.WarehouseLocation?.LocationName ?? "",
            WarehouseName = warehouseStock.WarehouseLocation?.Warehouse?.Name ?? "",
            ProductId = warehouseStock.ProductId,
            ProductName = warehouseStock.Product?.Name ?? "",
            ManufacturerId = warehouseStock.ManufacturerId,
            ManufacturerName = warehouseStock.Manufacturer?.Name ?? "",
            BottlesPerBox = warehouseStock.BottlesPerBox,
            QuantityInBottles = warehouseStock.QuantityInBottles,
            QuantityInBoxes = warehouseStock.QuantityInBoxes,
            PurchasePrice = warehouseStock.PurchasePrice,
            ExpirationDate = warehouseStock.ExpirationDate,
            CreatedAt = warehouseStock.CreatedAt
        };
    }

    // Debtor mappings
    public DebtorDto MapToDebtorDto(Debtor debtor)
    {
        return new DebtorDto
        {
            Id = debtor.Id,
            CompanyId = debtor.CompanyId,
            CompanyName = debtor.Company?.Name,
            DebtorName = debtor.DebtorName,
            Phone = debtor.Phone,
            Email = debtor.Email,
            TotalDebt = debtor.TotalDebt,
            PaidAmount = debtor.PaidAmount,
            RemainingDebt = debtor.RemainingDebt,
            DebtDate = debtor.DebtDate,
            LastPaymentDate = debtor.LastPaymentDate,
            Notes = debtor.Notes,
            IsPartnerCompany = debtor.Company != null && debtor.Company.IsPartner
        };
    }
}

