namespace WareHouseManagement.Application.DTOs;

public class WarehouseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

