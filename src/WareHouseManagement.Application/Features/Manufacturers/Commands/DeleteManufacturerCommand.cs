using MediatR;
using WareHouseManagement.Application.Common.Models;

namespace WareHouseManagement.Application.Features.Manufacturers.Commands;

public class DeleteManufacturerCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}

