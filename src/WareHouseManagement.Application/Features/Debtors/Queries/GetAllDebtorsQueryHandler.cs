using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Debtors.Queries;

public class GetAllDebtorsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllDebtorsQuery, Result<List<DebtorDto>>>
{
    public async Task<Result<List<DebtorDto>>> Handle(GetAllDebtorsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var debtors = await unitOfWork.Debtors.GetAllAsync();

            // თუ გვინდა მხოლოდ კონკრეტული კომპანიის დებიტორები
            if (request.CompanyId.HasValue)
            {
                debtors = debtors.Where(d => d.CompanyId == request.CompanyId.Value).ToList();
            }

            var debtorDtos = debtors.Select(d => new DebtorDto
            {
                Id = d.Id,
                CompanyId = d.CompanyId,
                DebtorName = d.DebtorName,
                Phone = d.Phone,
                Email = d.Email,
                TotalDebt = d.TotalDebt,
                PaidAmount = d.PaidAmount,
                RemainingDebt = d.RemainingDebt,
                DebtDate = d.DebtDate,
                LastPaymentDate = d.LastPaymentDate,
                Notes = d.Notes,
                IsPartnerCompany = d.CompanyId.HasValue,
                CreatedAt = d.CreatedAt
            }).ToList();

            return Result<List<DebtorDto>>.Success(debtorDtos);
        }
        catch (Exception ex)
        {
            return Result<List<DebtorDto>>.Failure($"შეცდომა დებიტორების მიღებისას: {ex.Message}");
        }
    }
}

