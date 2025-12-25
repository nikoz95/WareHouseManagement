using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Manufacturers.Queries;

public class GetManufacturerByIdQuery : IRequest<Result<ManufacturerDto>>
{
    public Guid Id { get; set; }
}

