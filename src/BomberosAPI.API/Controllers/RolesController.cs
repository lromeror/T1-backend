using BomberosAPI.API.Common.Responses;
using BomberosAPI.Application.Features.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BomberosAPI.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly RoleService _roleService;

    public RolesController(RoleService roleService) => _roleService = roleService;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RoleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var roles = await _roleService.GetAllAsync(ct);
        return Ok(ApiResponse<IReadOnlyList<RoleDto>>.Ok(roles));
    }
}
