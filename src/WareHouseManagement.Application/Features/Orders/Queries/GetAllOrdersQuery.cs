using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Orders.Queries;

public class GetAllOrdersQuery : IRequest<Result<IEnumerable<OrderDto>>>
{
    public Guid? CompanyId { get; set; }
    public Domain.Enums.OrderStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

