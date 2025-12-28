using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Features.Auth.Commands;

namespace WareHouseManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Login - ავტორიზაცია
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Username = request.Username,
            Password = request.Password,
            IpAddress = GetIpAddress()
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Refresh Token - ტოკენის განახლება
    /// </summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand
        {
            RefreshToken = request.RefreshToken,
            IpAddress = GetIpAddress()
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Test endpoint - Admin უფლების შემოწმება
    /// </summary>
    [HttpGet("test-admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult TestAdmin()
    {
        return Ok(new { message = "თქვენ ხართ ადმინისტრატორი!", user = User.Identity?.Name });
    }

    /// <summary>
    /// Test endpoint - Guest უფლების შემოწმება (ტრიპაჩების)
    /// </summary>
    [HttpGet("test-guest")]
    [Authorize(Roles = "Guest,Admin")]
    public IActionResult TestGuest()
    {
        return Ok(new { message = "თქვენ გაქვთ წვდომა!", user = User.Identity?.Name });
    }

    /// <summary>
    /// Test endpoint - Permission-ის შემოწმება
    /// </summary>
    [HttpGet("test-permission")]
    [Authorize(Policy = "Product.Read")]
    public IActionResult TestPermission()
    {
        return Ok(new { message = "თქვენ გაქვთ Product.Read უფლება!", user = User.Identity?.Name });
    }

    private string? GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        
        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}

