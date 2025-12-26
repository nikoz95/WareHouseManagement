using MediatR;
using Microsoft.Extensions.Options;
using WareHouseManagement.Application.Common.Interfaces;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Auth.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Refresh Token-ის პოვნა
            var refreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                return Result<LoginResponse>.Failure("არასწორი ან არააქტიური Refresh Token");
            }

            // იუზერის პოვნა როლებითა და უფლებებით
            var user = await _unitOfWork.Users.GetUserWithRolesAndPermissionsAsync(refreshToken.UserId);

            if (user == null || !user.IsActive)
            {
                return Result<LoginResponse>.Failure("მომხმარებელი ვერ მოიძებნა ან არააქტიურია");
            }

            // ძველი Refresh Token-ის გაუქმება
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedReason = "Replaced by new token";

            // ახალი Refresh Token-ის გენერაცია
            var newRefreshTokenString = _jwtTokenService.GenerateRefreshToken();
            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = newRefreshTokenString,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                IpAddress = request.IpAddress,
                CreatedAt = DateTime.UtcNow
            };

            refreshToken.ReplacedByToken = newRefreshTokenString;

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            // როლებისა და უფლებების მიღება
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => $"{rp.Permission.Resource}.{rp.Permission.Action}")
                .Distinct()
                .ToList();

            // ახალი Access Token-ის გენერაცია
            var accessToken = _jwtTokenService.GenerateAccessToken(user, roles, permissions);

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshTokenString,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Roles = roles,
                    Permissions = permissions
                }
            };

            return Result<LoginResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<LoginResponse>.Failure($"ტოკენის განახლების შეცდომა: {ex.Message}");
        }
    }
}

