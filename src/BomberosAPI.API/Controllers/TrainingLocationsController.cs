using BomberosAPI.API.Common.Responses;
using BomberosAPI.Application.Features.TrainingLocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BomberosAPI.API.Controllers;

[ApiController]
[Route("api/training-locations")]
[Authorize]
public class TrainingLocationsController : ControllerBase
{
    private readonly TrainingLocationService _service;

    public TrainingLocationsController(TrainingLocationService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TrainingLocationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var locations = await _service.GetAllAsync(ct);
        return Ok(ApiResponse<IReadOnlyList<TrainingLocationDto>>.Ok(locations));
    }
}
