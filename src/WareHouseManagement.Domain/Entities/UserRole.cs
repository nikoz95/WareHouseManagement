using WareHouseManagement.Domain.Common;

namespace WareHouseManagement.Domain.Entities;

/// <summary>
/// მომხმარებლის როლი - მრავალი-მრავალი კავშირი User-სა და Role-ს შორის
/// </summary>
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}

