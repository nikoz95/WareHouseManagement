using MediatR;
using Microsoft.Extensions.Options;
using WareHouseManagement.Application.Common.Interfaces;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork, 
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // იუზერის პოვნა
            var user = await _unitOfWork.Users.GetUserWithRolesAndPermissionsAsync(
                (await _unitOfWork.Users.GetByUsernameAsync(request.Username))?.Id ?? Guid.Empty);

            if (user == null || !user.IsActive)
            {
                return Result<LoginResponse>.Failure("არასწორი მომხმარებლის სახელი ან პაროლი");
            }

            // პაროლის შემოწმება
            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result<LoginResponse>.Failure("არასწორი მომხმარებლის სახელი ან პაროლი");
            }

            // როლებისა და უფლებების მიღება
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => $"{rp.Permission.Resource}.{rp.Permission.Action}")
                .Distinct()
                .ToList();

            // Access Token-ის გენერაცია
            var accessToken = _jwtTokenService.GenerateAccessToken(user, roles, permissions);

            // Refresh Token-ის გენერაცია
            var refreshTokenString = _jwtTokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshTokenString,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                IpAddress = request.IpAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);

            // ბოლო შესვლის დროის განახლება
            user.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
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
            return Result<LoginResponse>.Failure($"ავტორიზაციის შეცდომა: {ex.Message}");
        }
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        // BCrypt-ის გამოყენება პაროლის შესამოწმებლად
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}

