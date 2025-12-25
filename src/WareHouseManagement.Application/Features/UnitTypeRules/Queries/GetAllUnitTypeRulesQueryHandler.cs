using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.UnitTypeRules.Queries;

public class GetAllUnitTypeRulesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllUnitTypeRulesQuery, Result<List<UnitTypeRuleDto>>>
{
    public async Task<Result<List<UnitTypeRuleDto>>> Handle(GetAllUnitTypeRulesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rules = await unitOfWork.UnitTypeRules.GetAllAsync();

            // თუ გვინდა მხოლოდ აქტიური წესები
            if (request.OnlyActive.HasValue && request.OnlyActive.Value)
            {
                rules = rules.Where(r => r.IsActive).ToList();
            }

            var ruleDtos = rules.Select(r => new UnitTypeRuleDto
            {
                Id = r.Id,
                UnitType = r.UnitType,
                NameKa = r.NameKa,
                NameEn = r.NameEn,
                Abbreviation = r.Abbreviation,
                AllowOnlyWholeNumbers = r.AllowOnlyWholeNumbers,
                MinValue = r.MinValue,
                MaxValue = r.MaxValue,
                DefaultValue = r.DefaultValue,
                IsActive = r.IsActive,
                Description = r.Description,
                CreatedAt = r.CreatedAt
            }).ToList();

            return Result<List<UnitTypeRuleDto>>.Success(ruleDtos);
        }
        catch (Exception ex)
        {
            return Result<List<UnitTypeRuleDto>>.Failure($"შეცდომა წესების მიღებისას: {ex.Message}");
        }
    }
}

