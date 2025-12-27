using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.WarehouseStocks.Commands;

public class CreateWarehouseStockCommandHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    : IRequestHandler<CreateWarehouseStockCommand, Result<WarehouseStockDto>>
{
    public async Task<Result<WarehouseStockDto>> Handle(CreateWarehouseStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა WarehouseLocation
            var location = await unitOfWork.Warehouses.GetLocationByIdAsync(request.WarehouseLocationId);
            if (location == null)
            {
                return Result<WarehouseStockDto>.Failure("საწყობის ლოკაცია ვერ მოიძებნა");
            }

            // შევამოწმოთ არსებობს თუ არა პროდუქტი
            var product = await unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<WarehouseStockDto>.Failure("პროდუქტი ვერ მოიძებნა");
            }

            // შევამოწმოთ მწარმოებელი (თუ მითითებულია)
            if (request.ManufacturerId.HasValue)
            {
                var manufacturer = await unitOfWork.Manufacturers.GetByIdAsync(request.ManufacturerId.Value);
                if (manufacturer == null)
                {
                    return Result<WarehouseStockDto>.Failure("მწარმოებელი ვერ მოიძებნა");
                }
            }

            // შევქმნათ WarehouseStock
            var warehouseStock = new WarehouseStock
            {
                Id = Guid.NewGuid(),
                WarehouseLocationId = request.WarehouseLocationId,
                ProductId = request.ProductId,
                ManufacturerId = request.ManufacturerId,
                Quantity = request.Quantity,
                PurchasePrice = request.PurchasePrice ?? 0, // default 0 if not provided
                ExpirationDate = request.ExpirationDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await unitOfWork.Warehouses.AddStockAsync(warehouseStock);

            // თუ არის შეფუთვის ინფორმაცია
            if (request.UnitsPerPackage.HasValue && request.UnitsPerPackage.Value > 0)
            {
                var packagingDetails = new PackagingDetails
                {
                    Id = Guid.NewGuid(),
                    WarehouseStockId = warehouseStock.Id,
                    PackagingType = request.PackagingType ?? Domain.Enums.PackagingType.Box,
                    UnitsPerPackage = request.UnitsPerPackage.Value,
                    FullPackagesCount = request.FullPackagesCount ?? 0,
                    PartialPackagesCount = request.PartialPackagesCount ?? 0,
                    UnitsInPartialPackages = request.UnitsInPartialPackages ?? 0,
                    Notes = request.PackagingNotes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Note: PackagingDetails will be saved through UnitOfWork when we add the repository
                // For now, we'll use the DbContext directly through Infrastructure
            }

            // თუ ალკოჰოლური პროდუქტია
            if (!string.IsNullOrEmpty(request.BatchNumber) || 
                !string.IsNullOrEmpty(request.ExciseStampNumber) ||
                !string.IsNullOrEmpty(request.CertificateNumber) ||
                request.StorageTemperature.HasValue)
            {
                var alcoholicDetails = new AlcoholicStockDetails
                {
                    Id = Guid.NewGuid(),
                    WarehouseStockId = warehouseStock.Id,
                    BatchNumber = request.BatchNumber,
                    ExciseStampNumber = request.ExciseStampNumber,
                    CertificateNumber = request.CertificateNumber,
                    StorageTemperature = request.StorageTemperature,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Note: AlcoholicStockDetails will be saved through UnitOfWork when we add the repository
                // For now, we'll use the DbContext directly through Infrastructure
            }

            await unitOfWork.SaveChangesAsync();

            var stockDto = mapper.MapToWarehouseStockDto(warehouseStock);
            return Result<WarehouseStockDto>.Success(stockDto, "საწყობის მარაგი წარმატებით დაემატა");
        }
        catch (Exception ex)
        {
            return Result<WarehouseStockDto>.Failure($"შეცდომა საწყობის მარაგის დამატებისას: {ex.Message}");
        }
    }
}

