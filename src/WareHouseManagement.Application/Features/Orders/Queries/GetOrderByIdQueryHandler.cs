using MediatR;
using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Orders.Queries;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.Company)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
            {
                return Result<OrderDto>.Failure($"შეკვეთა ID {request.Id} ვერ მოიძებნა");
            }

            var orderDto = _mapper.MapToOrderDto(order);
            return Result<OrderDto>.Success(orderDto);
        }
        catch (Exception ex)
        {
            return Result<OrderDto>.Failure($"შეცდომა შეკვეთის ჩატვირთვისას: {ex.Message}");
        }
    }
}

