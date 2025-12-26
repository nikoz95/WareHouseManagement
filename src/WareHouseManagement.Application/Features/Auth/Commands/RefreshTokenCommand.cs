using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;

namespace WareHouseManagement.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<Result<LoginResponse>>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
}

