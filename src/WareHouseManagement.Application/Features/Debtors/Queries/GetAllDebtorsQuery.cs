using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Debtors.Queries;

public class GetAllDebtorsQuery : IRequest<Result<List<DebtorDto>>>
{
    public Guid? CompanyId { get; set; }
}
