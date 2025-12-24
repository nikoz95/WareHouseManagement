using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Products.Commands;

public class CreateProductCommandHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა პროდუქტი იგივე სახელით
            var existingProductByName = await unitOfWork.Products.FindAsync(p => p.Name == request.Name);
            if (existingProductByName.Any())
            {
                return Result<ProductDto>.Failure($"პროდუქტი სახელით '{request.Name}' უკვე არსებობს");
            }

            // შევამოწმოთ არსებობს თუ არა პროდუქტი იგივე ბარკოდით
            if (!string.IsNullOrEmpty(request.Barcode))
            {
                var existingProductByBarcode = await unitOfWork.Products.FindAsync(p => p.Barcode == request.Barcode);
                if (existingProductByBarcode.Any())
                {
                    return Result<ProductDto>.Failure($"პროდუქტი ბარკოდით '{request.Barcode}' უკვე არსებობს");
                }
            }

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

            await unitOfWork.Products.AddAsync(product);
            
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
                
                await unitOfWork.AlcoholicProducts.AddAsync(alcoholicProduct);
            }
            
            await unitOfWork.SaveChangesAsync();

            var productDto = mapper.MapToProductDto(product);
            return Result<ProductDto>.Success(productDto, "პროდუქტი წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Failure($"შეცდომა პროდუქტის შექმნისას: {ex.Message}");
        }
    }
}

