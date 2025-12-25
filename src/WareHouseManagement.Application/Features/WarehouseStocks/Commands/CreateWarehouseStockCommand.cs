using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.WarehouseStocks.Commands;

public class CreateWarehouseStockCommand : IRequest<Result<WarehouseStockDto>>
{
    public Guid WarehouseLocationId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ManufacturerId { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
    
    // შეფუთვის დეტალები (optional - ნებისმიერი პროდუქტისთვის)
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
}
