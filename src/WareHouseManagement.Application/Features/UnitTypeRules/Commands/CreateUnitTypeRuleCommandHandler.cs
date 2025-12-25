using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.UnitTypeRules.Commands;

public class CreateUnitTypeRuleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUnitTypeRuleCommand, Result<UnitTypeRuleDto>>
{
    public async Task<Result<UnitTypeRuleDto>> Handle(CreateUnitTypeRuleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა წესი ამ UnitType-ისთვის
            var existingRules = await unitOfWork.UnitTypeRules.FindAsync(r => r.UnitType == request.UnitType);
            if (existingRules.Any())
            {
                return Result<UnitTypeRuleDto>.Failure($"წესი საზომი ერთეულისთვის '{request.UnitType}' უკვე არსებობს");
            }

            var rule = new UnitTypeRule
            {
                Id = Guid.NewGuid(),
                UnitType = request.UnitType,
                NameKa = request.NameKa,
                NameEn = request.NameEn,
                Abbreviation = request.Abbreviation,
                AllowOnlyWholeNumbers = request.AllowOnlyWholeNumbers,
                MinValue = request.MinValue,
                MaxValue = request.MaxValue,
                DefaultValue = request.DefaultValue,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await unitOfWork.UnitTypeRules.AddAsync(rule);
            await unitOfWork.SaveChangesAsync();

            var ruleDto = new UnitTypeRuleDto
            {
                Id = rule.Id,
                UnitType = rule.UnitType,
                NameKa = rule.NameKa,
                NameEn = rule.NameEn,
                Abbreviation = rule.Abbreviation,
                AllowOnlyWholeNumbers = rule.AllowOnlyWholeNumbers,
                MinValue = rule.MinValue,
                MaxValue = rule.MaxValue,
                DefaultValue = rule.DefaultValue,
                IsActive = rule.IsActive,
                Description = rule.Description,
                CreatedAt = rule.CreatedAt
            };

            return Result<UnitTypeRuleDto>.Success(ruleDto, "საზომი ერთეულის წესი წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<UnitTypeRuleDto>.Failure($"შეცდომა წესის შექმნისას: {ex.Message}");
        }
    }
}

