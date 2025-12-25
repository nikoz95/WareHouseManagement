using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.UnitTypeRules.Queries;

public class GetAllUnitTypeRulesQuery : IRequest<Result<List<UnitTypeRuleDto>>>
{
    public bool? OnlyActive { get; set; } = true;
}

