using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Application.Features.Orders.Commands;

public class UpdateOrderCommand : IRequest<Result<OrderDto>>
{
    public Guid Id { get; set; }
    public OrderStatus? Status { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public decimal? PaidAmount { get; set; }
    public string? Notes { get; set; }
}

