using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Domain.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<RefreshToken?> GetActiveTokenAsync(Guid userId, string token);
    Task<IEnumerable<RefreshToken>> GetUserActiveTokensAsync(Guid userId);
    Task RevokeTokenAsync(string token, string reason);
    Task RevokeAllUserTokensAsync(Guid userId, string reason);
    Task CleanupExpiredTokensAsync();
}

