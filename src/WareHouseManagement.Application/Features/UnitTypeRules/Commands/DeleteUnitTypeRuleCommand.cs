using MediatR;
using WareHouseManagement.Application.Common.Models;

namespace WareHouseManagement.Application.Features.UnitTypeRules.Commands;

public class DeleteUnitTypeRuleCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}

