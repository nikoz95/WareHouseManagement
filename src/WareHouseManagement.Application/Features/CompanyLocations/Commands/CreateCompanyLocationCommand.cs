using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.CompanyLocations.Commands;

public class CreateCompanyLocationCommand : IRequest<Result<CompanyLocationDto>>
{
    public Guid CompanyId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? ContactPerson { get; set; }
}
