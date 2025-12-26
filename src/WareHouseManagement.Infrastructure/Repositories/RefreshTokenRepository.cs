using Microsoft.EntityFrameworkCore;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task<RefreshToken?> GetActiveTokenAsync(Guid userId, string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => 
                rt.UserId == userId && 
                rt.Token == token && 
                !rt.IsRevoked && 
                rt.ExpiresAt > DateTime.UtcNow);
    }

    public async Task<IEnumerable<RefreshToken>> GetUserActiveTokensAsync(Guid userId)
    {
        return await _context.RefreshTokens
            .Where(rt => 
                rt.UserId == userId && 
                !rt.IsRevoked && 
                rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task RevokeTokenAsync(string token, string reason)
    {
        var refreshToken = await GetByTokenAsync(token);
        if (refreshToken != null && !refreshToken.IsRevoked)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedReason = reason;
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RevokeAllUserTokensAsync(Guid userId, string reason)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedReason = reason;
            token.RevokedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task CleanupExpiredTokensAsync()
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }
}

