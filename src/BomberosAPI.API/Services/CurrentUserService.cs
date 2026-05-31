using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BomberosAPI.Application.Common.Interfaces;

namespace BomberosAPI.API.Services;

/// <summary>
/// Lee la identidad del usuario autenticado desde los claims del JWT en el HttpContext actual.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId
    {
        get
        {
            var value = User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }
    }

    public string Email =>
        User?.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty;

    public string FirstName =>
        User?.FindFirstValue(JwtRegisteredClaimNames.GivenName) ?? string.Empty;

    public string LastName =>
        User?.FindFirstValue(JwtRegisteredClaimNames.FamilyName) ?? string.Empty;

    public IReadOnlyList<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? [];

    public bool IsInRole(string role) =>
        User?.IsInRole(role) ?? false;
}
