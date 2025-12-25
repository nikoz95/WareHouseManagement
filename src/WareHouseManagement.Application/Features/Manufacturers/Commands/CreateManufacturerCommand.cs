using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Manufacturers.Commands;

public class CreateManufacturerCommand : IRequest<Result<ManufacturerDto>>
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? ContactInfo { get; set; }
    public string? Description { get; set; }
}
