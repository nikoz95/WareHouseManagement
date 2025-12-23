using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Orders.Commands;

public class CreateOrderCommand : IRequest<Result<OrderDto>>
{
    public Guid? CompanyId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public string? Notes { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

