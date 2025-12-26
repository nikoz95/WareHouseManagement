using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// უფლება - სისტემაში შესაძლო ქმედება/უფლება
/// </summary>
public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty; // მაგ: "Product", "Order", "Company"
    public string Action { get; set; } = string.Empty; // მაგ: "Read", "Create", "Update", "Delete"
    
    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

