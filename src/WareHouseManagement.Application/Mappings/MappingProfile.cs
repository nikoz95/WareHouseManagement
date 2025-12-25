using Riok.Mapperly.Abstractions;
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
            CreatedAt = product.CreatedAt
        };

        // დამატებითი დეტალური ინფორმაცია (თუ არსებობს)
        if (product.ProductDetails != null)
        {
            dto.HasDetails = true;
            dto.CountryOfOrigin = product.ProductDetails.CountryOfOrigin;
            dto.ProductType = product.ProductDetails.ProductType;
            dto.ShelfLifeMonths = product.ProductDetails.ShelfLifeMonths;
            dto.AdditionalNotes = product.ProductDetails.AdditionalNotes;
            
            // ალკოჰოლური დეტალები (თუ არსებობს)
            if (product.ProductDetails.AlcoholicDetails != null)
            {
                dto.IsAlcoholic = true;
                dto.AlcoholPercentage = product.ProductDetails.AlcoholicDetails.AlcoholPercentage;
                dto.Region = product.ProductDetails.AlcoholicDetails.Region;
                dto.ServingTemperature = product.ProductDetails.AlcoholicDetails.ServingTemperature;
                dto.QualityClass = product.ProductDetails.AlcoholicDetails.QualityClass;
            }
        }

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
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            TotalPrice = orderItem.TotalPrice
        };
    }

    // WarehouseStock mappings - with optional PackagingDetails and AlcoholicDetails
    public WarehouseStockDto MapToWarehouseStockDto(WarehouseStock warehouseStock)
    {
        var dto = new WarehouseStockDto
        {
            Id = warehouseStock.Id,
            WarehouseLocationId = warehouseStock.WarehouseLocationId,
            WarehouseLocationName = warehouseStock.WarehouseLocation?.LocationName ?? "",
            WarehouseName = warehouseStock.WarehouseLocation?.Warehouse?.Name ?? "",
            ProductId = warehouseStock.ProductId,
            ProductName = warehouseStock.Product?.Name ?? "",
            ManufacturerId = warehouseStock.ManufacturerId,
            ManufacturerName = warehouseStock.Manufacturer?.Name ?? "",
            Quantity = warehouseStock.Quantity,
            PurchasePrice = warehouseStock.PurchasePrice,
            ExpirationDate = warehouseStock.ExpirationDate,
            CreatedAt = warehouseStock.CreatedAt
        };

        // Check if has packaging details (უნივერსალური - ნებისმიერი პროდუქტისთვის)
        if (warehouseStock.PackagingDetails != null)
        {
            dto.PackagingType = warehouseStock.PackagingDetails.PackagingType;
            dto.UnitsPerPackage = warehouseStock.PackagingDetails.UnitsPerPackage;
            dto.FullPackagesCount = warehouseStock.PackagingDetails.FullPackagesCount;
            dto.PartialPackagesCount = warehouseStock.PackagingDetails.PartialPackagesCount;
            dto.UnitsInPartialPackages = warehouseStock.PackagingDetails.UnitsInPartialPackages;
            dto.TotalPackagesCount = warehouseStock.PackagingDetails.TotalPackagesCount;
            dto.TotalUnitsCount = warehouseStock.PackagingDetails.TotalUnitsCount;
            dto.PackagingNotes = warehouseStock.PackagingDetails.Notes;
        }

        // Check if has alcoholic details
        if (warehouseStock.AlcoholicDetails != null)
        {
            dto.StockType = "Alcoholic";
            dto.BatchNumber = warehouseStock.AlcoholicDetails.BatchNumber;
            dto.ExciseStampNumber = warehouseStock.AlcoholicDetails.ExciseStampNumber;
            dto.CertificateNumber = warehouseStock.AlcoholicDetails.CertificateNumber;
            dto.StorageTemperature = warehouseStock.AlcoholicDetails.StorageTemperature;
        }
        else
        {
            dto.StockType = "General";
        }

        return dto;
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

    // Manufacturer mappings
    public ManufacturerDto MapToManufacturerDto(Manufacturer manufacturer)
    {
        return new ManufacturerDto
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            Country = manufacturer.Country,
            ContactInfo = manufacturer.ContactInfo,
            Description = manufacturer.Description,
            CreatedAt = manufacturer.CreatedAt
        };
    }

    // Warehouse mappings
    public WarehouseDto MapToWarehouseDto(Warehouse warehouse)
    {
        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Address = warehouse.Address,
            Description = warehouse.Description,
            CreatedAt = warehouse.CreatedAt
        };
    }

    // WarehouseLocation mappings
    public WarehouseLocationDto MapToWarehouseLocationDto(WarehouseLocation location)
    {
        return new WarehouseLocationDto
        {
            Id = location.Id,
            WarehouseId = location.WarehouseId,
            WarehouseName = location.Warehouse?.Name ?? string.Empty, // Warehouse may be null if not included in query
            LocationName = location.LocationName,
            Description = location.Description,
            CreatedAt = location.CreatedAt
        };
    }
}

