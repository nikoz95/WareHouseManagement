using MediatR;
using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Orders.Queries;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<IEnumerable<OrderDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ordersQuery = _unitOfWork.Orders.GetQueryable();

            // ფი��ტრაცია
            if (request.CompanyId.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CompanyId == request.CompanyId.Value);
            }

            if (request.Status.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.Status == request.Status.Value);
            }

            if (request.FromDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate <= request.ToDate.Value);
            }

            // Include related data
            ordersQuery = ordersQuery
                .Include(o => o.Company)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product);

            var orders = await ordersQuery
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync(cancellationToken);

            var orderDtos = orders.Select(_mapper.MapToOrderDto);
            return Result<IEnumerable<OrderDto>>.Success(orderDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OrderDto>>.Failure($"შეცდომა შეკვეთების ჩატვირთვისას: {ex.Message}");
        }
    }
}

