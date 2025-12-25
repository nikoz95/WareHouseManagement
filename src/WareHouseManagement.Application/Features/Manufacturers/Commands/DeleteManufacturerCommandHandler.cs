using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Manufacturers.Commands;

public class DeleteManufacturerCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteManufacturerCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var manufacturer = await unitOfWork.Manufacturers.GetByIdAsync(request.Id);
            if (manufacturer == null)
            {
                return Result<bool>.Failure("მწარმოებელი ვერ მოიძებნა");
            }

            await unitOfWork.Manufacturers.DeleteAsync(manufacturer);
            await unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true, "მწარმოებელი წარმატებით წაიშალა");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"შეცდომა მწარმოებლის წაშლისას: {ex.Message}");
        }
    }
}

