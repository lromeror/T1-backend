using BomberosAPI.API.Common.Responses;
using BomberosAPI.Application.Features.HealthPersonnel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BomberosAPI.API.Controllers;

[ApiController]
[Route("api/health-personnel")]
[Authorize]
public class HealthPersonnelController : ControllerBase
{
    private readonly HealthPersonnelService _service;

    public HealthPersonnelController(HealthPersonnelService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<HealthPersonnelDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(ApiResponse<IReadOnlyList<HealthPersonnelDto>>.Ok(items));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<HealthPersonnelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var hp = await _service.GetByIdAsync(id, ct);
        return Ok(ApiResponse<HealthPersonnelDto>.Ok(hp));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<HealthPersonnelDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateHealthPersonnelRequest request, CancellationToken ct)
    {
        var hp = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = hp.HealthPersonnelId },
            ApiResponse<HealthPersonnelDto>.Created(hp));
    }
}