using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.WarehouseLocations.Queries;

public class GetAllWarehouseLocationsQuery : IRequest<Result<List<WarehouseLocationDto>>>
{
    public Guid? WarehouseId { get; set; }
}

