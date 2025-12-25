using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Orders.Queries;

public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
{
    public Guid Id { get; set; }
}

