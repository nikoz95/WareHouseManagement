using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.CompanyLocations.Queries;

public class GetAllCompanyLocationsQuery : IRequest<Result<List<CompanyLocationDto>>>
{
    public Guid? CompanyId { get; set; }
}

