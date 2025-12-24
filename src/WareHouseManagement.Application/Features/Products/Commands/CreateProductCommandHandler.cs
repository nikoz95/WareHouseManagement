using AutoMapper;
using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Barcode = request.Barcode,
                Price = request.Price,
                UnitTypeRuleId = request.UnitTypeRuleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Products.AddAsync(product);
            
            // თუ პროდუქტი ალკოჰოლურია, შევქმნათ AlcoholicProduct ჩანაწერი
            if (request.IsAlcoholic && request.AlcoholPercentage.HasValue)
            {
                var alcoholicProduct = new AlcoholicProduct
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    AlcoholPercentage = request.AlcoholPercentage.Value,
                    AlcoholType = request.AlcoholType,
                    CountryOfOrigin = request.CountryOfOrigin,
                    ShelfLifeMonths = request.ShelfLifeMonths,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await _unitOfWork.AlcoholicProducts.AddAsync(alcoholicProduct);
            }
            
            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            return Result<ProductDto>.Success(productDto, "პროდუქტი წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Failure($"შეცდომა პროდუქტის შექმნისას: {ex.Message}");
        }
    }
}

