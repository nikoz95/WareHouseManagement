using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Application.DTOs;

public class UnitTypeRuleDto
{
    public Guid Id { get; set; }
    public UnitType UnitType { get; set; }
    public string NameKa { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public bool AllowOnlyWholeNumbers { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public decimal? DefaultValue { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

