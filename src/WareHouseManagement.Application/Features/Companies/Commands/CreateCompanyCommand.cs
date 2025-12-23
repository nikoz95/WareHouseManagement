using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Companies.Commands;

public class CreateCompanyCommand : IRequest<Result<CompanyDto>>
{
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Domain.Enums.CompanyType CompanyType { get; set; }
    public bool IsPartner { get; set; }
}

