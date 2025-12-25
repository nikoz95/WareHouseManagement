using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Manufacturers.Queries;

public class GetAllManufacturersQuery : IRequest<Result<List<ManufacturerDto>>>
{
}

