using MediatR;
using WareHouseManagement.Application.Common.Models;

namespace WareHouseManagement.Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
