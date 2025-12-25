using MediatR;
using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Warehouses.Queries;

public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, Result<WarehouseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<WarehouseDto>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = await _unitOfWork.Warehouses.GetQueryable()
                .Include(w => w.WarehouseLocations)
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (warehouse == null)
            {
                return Result<WarehouseDto>.Failure($"საწყობი ID {request.Id} ვერ მოიძებნა");
            }

            var warehouseDto = _mapper.MapToWarehouseDto(warehouse);
            return Result<WarehouseDto>.Success(warehouseDto);
        }
        catch (Exception ex)
        {
            return Result<WarehouseDto>.Failure($"შეცდომა საწყობის ჩატვირთვისას: {ex.Message}");
        }
    }
}

