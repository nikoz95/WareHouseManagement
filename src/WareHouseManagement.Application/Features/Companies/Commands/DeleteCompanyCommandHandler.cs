using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Companies.Commands;

public class DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCompanyCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var company = await unitOfWork.Companies.GetByIdAsync(request.Id);
            if (company == null)
            {
                return Result<bool>.Failure("კომპანია ვერ მოიძებნა");
            }

            await unitOfWork.Companies.DeleteAsync(company);
            await unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true, "კომპანია წარმატებით წაიშალა");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"შეცდომა კომპანიის წაშლისას: {ex.Message}");
        }
    }
}

