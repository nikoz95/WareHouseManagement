﻿using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Products.Queries;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var productDtos = products.Select(_mapper.MapToProductDto);
            return Result<IEnumerable<ProductDto>>.Success(productDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProductDto>>.Failure($"შეცდომა პროდუქტების ჩატვირთვისას: {ex.Message}");
        }
    }
}

