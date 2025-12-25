using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.WarehouseLocations.Queries;

public class GetAllWarehouseLocationsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllWarehouseLocationsQuery, Result<List<WarehouseLocationDto>>>
{
    public async Task<Result<List<WarehouseLocationDto>>> Handle(GetAllWarehouseLocationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<WarehouseLocationDto> locationDtos;

            if (request.WarehouseId.HasValue)
            {
                // მხოლოდ კონკრეტული საწყობის ლოკაციები
                var warehouse = await unitOfWork.Warehouses.GetByIdAsync(request.WarehouseId.Value);
                if (warehouse == null)
                {
                    return Result<List<WarehouseLocationDto>>.Failure("საწყობი ვერ მოიძებნა");
                }

                locationDtos = warehouse.WarehouseLocations
                    .Where(wl => !wl.IsDeleted)
                    .Select(wl => new WarehouseLocationDto
                    {
                        Id = wl.Id,
                        WarehouseId = wl.WarehouseId,
                        WarehouseName = warehouse.Name,
                        LocationName = wl.LocationName,
                        Description = wl.Description,
                        CreatedAt = wl.CreatedAt
                    })
                    .ToList();
            }
            else
            {
                // ყველა ლოკაცია ყველა საწყობიდან
                var warehouses = await unitOfWork.Warehouses.GetAllAsync();
                
                locationDtos = warehouses
                    .SelectMany(w => w.WarehouseLocations
                        .Where(wl => !wl.IsDeleted)
                        .Select(wl => new WarehouseLocationDto
                        {
                            Id = wl.Id,
                            WarehouseId = wl.WarehouseId,
                            WarehouseName = w.Name,
                            LocationName = wl.LocationName,
                            Description = wl.Description,
                            CreatedAt = wl.CreatedAt
                        }))
                    .ToList();
            }

            return Result<List<WarehouseLocationDto>>.Success(locationDtos);
        }
        catch (Exception ex)
        {
            return Result<List<WarehouseLocationDto>>.Failure($"შეცდომა ლოკაციების მიღებისას: {ex.Message}");
        }
    }
}

