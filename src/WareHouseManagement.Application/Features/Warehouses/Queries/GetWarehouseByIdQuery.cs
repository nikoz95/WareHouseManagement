using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Warehouses.Queries;

public class GetWarehouseByIdQuery : IRequest<Result<WarehouseDto>>
{
    public Guid Id { get; set; }
}

