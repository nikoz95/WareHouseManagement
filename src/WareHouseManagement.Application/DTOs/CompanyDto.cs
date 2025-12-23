using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Application.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public CompanyType CompanyType { get; set; }
    public string CompanyTypeDescription { get; set; } = string.Empty;
    public bool IsPartner { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCompanyDto
{
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public CompanyType CompanyType { get; set; }
    public bool IsPartner { get; set; }
}

public class UpdateCompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public CompanyType CompanyType { get; set; }
    public bool IsPartner { get; set; }
}

