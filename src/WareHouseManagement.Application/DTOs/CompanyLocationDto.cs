namespace WareHouseManagement.Application.DTOs;

public class CompanyLocationDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? ContactPerson { get; set; }
    public DateTime CreatedAt { get; set; }
}

