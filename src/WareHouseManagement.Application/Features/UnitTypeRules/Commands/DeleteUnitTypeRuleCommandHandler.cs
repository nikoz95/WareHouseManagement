using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.UnitTypeRules.Commands;

public class DeleteUnitTypeRuleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteUnitTypeRuleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteUnitTypeRuleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var rule = await unitOfWork.UnitTypeRules.GetByIdAsync(request.Id);
            if (rule == null)
            {
                return Result<bool>.Failure("საზომი ერთეულის წესი ვერ მოიძებნა");
            }

            await unitOfWork.UnitTypeRules.DeleteAsync(rule);
            await unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true, "საზომი ერთეულის წესი წარმატებით წაიშალა");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"შეცდომა წესის წაშლისას: {ex.Message}");
        }
    }
}

