using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.WarehouseLocations.Commands;

public class CreateWarehouseLocationCommand : IRequest<Result<WarehouseLocationDto>>
{
    public Guid WarehouseId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
