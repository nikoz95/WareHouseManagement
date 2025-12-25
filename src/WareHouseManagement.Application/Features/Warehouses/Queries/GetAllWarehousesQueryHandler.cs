using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Warehouses.Queries;

public class GetAllWarehousesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllWarehousesQuery, Result<List<WarehouseDto>>>
{
    public async Task<Result<List<WarehouseDto>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouses = await unitOfWork.Warehouses.GetAllAsync();

            var warehouseDtos = warehouses.Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Address = w.Address,
                Description = w.Description,
                CreatedAt = w.CreatedAt
            }).ToList();

            return Result<List<WarehouseDto>>.Success(warehouseDtos);
        }
        catch (Exception ex)
        {
            return Result<List<WarehouseDto>>.Failure($"შეცდომა საწყობების მიღებისას: {ex.Message}");
        }
    }
}

