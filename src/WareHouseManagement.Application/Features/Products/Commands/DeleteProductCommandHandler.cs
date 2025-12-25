using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Products.Commands;

public class DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await unitOfWork.Products.GetByIdAsync(request.Id);
            if (product == null)
            {
                return Result<bool>.Failure("პროდუქტი ვერ მოიძებნა");
            }

            await unitOfWork.Products.DeleteAsync(product);
            await unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true, "პროდუქტი წარმატებით წაიშალა");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"შეცდომა პროდუქტის წაშლისას: {ex.Message}");
        }
    }
}

