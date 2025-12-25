using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Warehouses.Commands;

public class CreateWarehouseCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
{
    public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა საწყობი იგივე სახელით
            var existingWarehouses = await unitOfWork.Warehouses.FindAsync(w => w.Name == request.Name);
            if (existingWarehouses.Any())
            {
                return Result<WarehouseDto>.Failure($"საწყობი სახელით '{request.Name}' უკვე არსებობს");
            }

            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address ?? string.Empty,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await unitOfWork.Warehouses.AddAsync(warehouse);
            await unitOfWork.SaveChangesAsync();

            var warehouseDto = new WarehouseDto
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Address = warehouse.Address,
                Description = warehouse.Description,
                CreatedAt = warehouse.CreatedAt
            };

            return Result<WarehouseDto>.Success(warehouseDto, "საწყობი წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<WarehouseDto>.Failure($"შეცდომა საწყობის შექმნისას: {ex.Message}");
        }
    }
}

