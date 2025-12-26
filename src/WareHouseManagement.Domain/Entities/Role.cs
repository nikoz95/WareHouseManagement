using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// როლი - მომხმარებლის როლი/პოზიცია
/// </summary>
public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

