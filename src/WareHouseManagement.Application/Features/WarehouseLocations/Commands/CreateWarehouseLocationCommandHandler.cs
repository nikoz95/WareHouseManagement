using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.WarehouseLocations.Commands;

public class CreateWarehouseLocationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWarehouseLocationCommand, Result<WarehouseLocationDto>>
{
    public async Task<Result<WarehouseLocationDto>> Handle(CreateWarehouseLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა საწყობი
            var warehouse = await unitOfWork.Warehouses.GetByIdAsync(request.WarehouseId);
            if (warehouse == null)
            {
                return Result<WarehouseLocationDto>.Failure("საწყობი ვერ მოიძებნა");
            }

            // შევამოწმოთ არსებობს თუ არა ლოკაცია იმავე სახელით ამ საწყობში
            var existingLocations = await unitOfWork.Warehouses.FindAsync(w => w.Id == request.WarehouseId);
            var existingLocation = existingLocations
                .SelectMany(w => w.WarehouseLocations)
                .FirstOrDefault(wl => wl.LocationName == request.LocationName && !wl.IsDeleted);

            if (existingLocation != null)
            {
                return Result<WarehouseLocationDto>.Failure($"ლოკაცია სახელით '{request.LocationName}' უკვე არსებობს ამ საწყობში");
            }

            var location = new WarehouseLocation
            {
                Id = Guid.NewGuid(),
                WarehouseId = request.WarehouseId,
                LocationName = request.LocationName,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Note: We need to add a method to save WarehouseLocation
            // For now, we'll add it through the warehouse
            warehouse.WarehouseLocations.Add(location);
            await unitOfWork.SaveChangesAsync();

            var locationDto = new WarehouseLocationDto
            {
                Id = location.Id,
                WarehouseId = location.WarehouseId,
                WarehouseName = warehouse.Name,
                LocationName = location.LocationName,
                Description = location.Description,
                CreatedAt = location.CreatedAt
            };

            return Result<WarehouseLocationDto>.Success(locationDto, "საწყობის ლოკაცია წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<WarehouseLocationDto>.Failure($"შეცდომა ლოკაციის შექმნისას: {ex.Message}");
        }
    }
}

