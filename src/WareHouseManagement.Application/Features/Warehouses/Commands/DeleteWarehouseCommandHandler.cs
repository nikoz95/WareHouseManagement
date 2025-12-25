using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Warehouses.Commands;

public class DeleteWarehouseCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteWarehouseCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = await unitOfWork.Warehouses.GetByIdAsync(request.Id);
            if (warehouse == null)
            {
                return Result<bool>.Failure("საწყობი ვერ მოიძებნა");
            }

            await unitOfWork.Warehouses.DeleteAsync(warehouse);
            await unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true, "საწყობი წარმატებით წაიშალა");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"შეცდომა საწყობის წაშლისას: {ex.Message}");
        }
    }
}

