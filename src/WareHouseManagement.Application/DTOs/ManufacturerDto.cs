namespace WareHouseManagement.Application.DTOs;

public class ManufacturerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? ContactInfo { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

