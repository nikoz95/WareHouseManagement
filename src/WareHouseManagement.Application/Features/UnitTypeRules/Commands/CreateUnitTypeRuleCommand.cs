using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Application.Features.UnitTypeRules.Commands;

public class CreateUnitTypeRuleCommand : IRequest<Result<UnitTypeRuleDto>>
{
    public UnitType UnitType { get; set; }
    public string NameKa { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public bool AllowOnlyWholeNumbers { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public decimal? DefaultValue { get; set; }
    public string? Description { get; set; }
}
