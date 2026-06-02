using BomberosAPI.API.Common.Responses;
using BomberosAPI.Application.Features.MedicalHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BomberosAPI.API.Controllers;

[ApiController]
[Route("api/medical-history")]
[Authorize]
public class MedicalHistoryController : ControllerBase
{
    private readonly MedicalHistoryService _service;

    public MedicalHistoryController(MedicalHistoryService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<MedicalHistoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(ApiResponse<IReadOnlyList<MedicalHistoryDto>>.Ok(items));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var mh = await _service.GetByIdAsync(id, ct);
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(mh));
    }

    [HttpGet("by-trainee/{traineeId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTrainee(Guid traineeId, CancellationToken ct)
    {
        var mh = await _service.GetByTraineeAsync(traineeId, ct);
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(mh));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MedicalHistoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateMedicalHistoryRequest request, CancellationToken ct)
    {
        var mh = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = mh.MedicalHistoryId },
            ApiResponse<MedicalHistoryDto>.Created(mh));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMedicalHistoryRequest request, CancellationToken ct)
    {
        var mh = await _service.UpdateAsync(id, request, ct);
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(mh));
    }
}