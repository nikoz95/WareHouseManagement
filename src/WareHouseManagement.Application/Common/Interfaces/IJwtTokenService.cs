using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Application.Common.Interfaces;

/// <summary>
/// JWT Token Service - ტოკენების გენერაციისა და ვალიდაციის სერვისი
/// </summary>
public interface IJwtTokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    Guid? ValidateAccessToken(string token);
    bool ValidateRefreshToken(string token);
}

