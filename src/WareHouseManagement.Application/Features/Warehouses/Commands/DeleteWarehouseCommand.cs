using MediatR;
using WareHouseManagement.Application.Common.Models;

namespace WareHouseManagement.Application.Features.Warehouses.Commands;

public class DeleteWarehouseCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
