using WareHouseManagement.Domain.Common;
using WareHouseManagement.Domain.Enums;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// საზომი ერთეულის წესები და კონფიგურაცია
/// </summary>
public class UnitTypeRule : BaseEntity
{
    /// <summary>
    /// საზომი ერთეულის ტიპი
    /// </summary>
    public UnitType UnitType { get; set; }
    
    /// <summary>
    /// საზომი ერთეულის სახელი ქართულად
    /// </summary>
    public string NameKa { get; set; } = string.Empty;
    
    /// <summary>
    /// საზომი ერთეულის სახელი ინგლისურად
    /// </summary>
    public string NameEn { get; set; } = string.Empty;
    
    /// <summary>
    /// შეკვეცილი ფორმა (მაგ: "ც", "ლ", "კგ")
    /// </summary>
    public string Abbreviation { get; set; } = string.Empty;
    
    /// <summary>
    /// უნდა იყოს თუ არა მხოლოდ მთელი რიცხვი
    /// </summary>
    public bool AllowOnlyWholeNumbers { get; set; }
    
    /// <summary>
    /// მინიმალური მნიშვნელობა
    /// </summary>
    public decimal? MinValue { get; set; }
    
    /// <summary>
    /// მაქსიმალური მნიშვნელობა
    /// </summary>
    public decimal? MaxValue { get; set; }
    
    /// <summary>
    /// ნაგულისხმევი მნიშვნელობა
    /// </summary>
    public decimal? DefaultValue { get; set; }
    
    /// <summary>
    /// არის თუ არა აქტიური (გამოიყენება თუ არა)
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// დამატებითი შენიშვნები
    /// </summary>
    public string? Description { get; set; }
}

