using MediatR;
using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.UnitTypeRule)
                .Include(p => p.ProductDetails!)
                    .ThenInclude(pd => pd!.AlcoholicDetails)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result<ProductDto>.Failure($"პროდუქტი ID {request.Id} ვერ მოიძებნა");
            }

            var productDto = _mapper.MapToProductDto(product);
            return Result<ProductDto>.Success(productDto);
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Failure($"შეცდომა პროდუქტის ჩატვირთვისას: {ex.Message}");
        }
    }
}

