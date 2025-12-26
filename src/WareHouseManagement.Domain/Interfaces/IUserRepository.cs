using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserWithRolesAsync(Guid userId);
    Task<User?> GetUserWithRolesAndPermissionsAsync(Guid userId);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}

