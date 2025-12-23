using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Companies.Queries;

public class GetAllCompaniesQuery : IRequest<Result<IEnumerable<CompanyDto>>>
{
}

