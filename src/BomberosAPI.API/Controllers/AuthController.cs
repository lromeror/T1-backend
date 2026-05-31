using BomberosAPI.API.Common.Responses;
using BomberosAPI.Application.Common.Interfaces;
using BomberosAPI.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BomberosAPI.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ICurrentUserService _currentUser;

    public AuthController(AuthService authService, ICurrentUserService currentUser)
    {
        _authService = authService;
        _currentUser = currentUser;
    }

    /// <summary>Autentica un usuario y retorna un JWT firmado.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request, ct);
        return Ok(ApiResponse<LoginResult>.Ok(result, "Login successful."));
    }

    /// <summary>Retorna la identidad del usuario autenticado desde el token JWT.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<CurrentUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    public IActionResult Me()
    {
        var response = new CurrentUserResponse(
            _currentUser.UserId,
            _currentUser.Email,
            _currentUser.FirstName,
            _currentUser.LastName,
            _currentUser.Roles);

        return Ok(ApiResponse<CurrentUserResponse>.Ok(response));
    }
}

public record CurrentUserResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    IReadOnlyList<string> Roles
);
