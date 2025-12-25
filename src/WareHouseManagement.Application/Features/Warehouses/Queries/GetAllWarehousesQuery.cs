using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Warehouses.Queries;

public class GetAllWarehousesQuery : IRequest<Result<List<WarehouseDto>>>
{
}

