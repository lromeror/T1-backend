using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Domain.Repositories;
using FluentValidation;
using EnvironmentalDataEntity = BomberosAPI.Domain.Entities.EnvironmentalData;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.EnvironmentalData;

public class EnvironmentalDataService
{
    private readonly IEnvironmentalDataRepository _repo;
    private readonly ITrainingSessionRepository _sessionRepo;
    private readonly IUserRepository _userRepo;
    private readonly IValidator<CreateEnvironmentalDataRequest> _createValidator;

    public EnvironmentalDataService(
        IEnvironmentalDataRepository repo,
        ITrainingSessionRepository sessionRepo,
        IUserRepository userRepo,
        IValidator<CreateEnvironmentalDataRequest> createValidator)
    {
        _repo = repo;
        _sessionRepo = sessionRepo;
        _userRepo = userRepo;
        _createValidator = createValidator;
    }

    public async Task<IReadOnlyList<EnvironmentalDataDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<EnvironmentalDataDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("EnvironmentalData", id);
        return ToDto(e);
    }

    public async Task<IReadOnlyList<EnvironmentalDataDto>> GetBySessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        var items = await _repo.GetBySessionAsync(sessionId, ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<EnvironmentalDataDto> CreateAsync(CreateEnvironmentalDataRequest request, CancellationToken ct = default)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }

        if (await _sessionRepo.GetByIdAsync(request.TrainingSessionId, ct) is null)
            throw new NotFoundException("TrainingSession", request.TrainingSessionId);

        if (await _userRepo.GetByIdAsync(request.RegisteredByUserId, ct) is null)
            throw new NotFoundException("User", request.RegisteredByUserId);

        var e = new EnvironmentalDataEntity
        {
            EnvironmentalDataId = Guid.NewGuid(),
            TrainingSessionId = request.TrainingSessionId,
            RegisteredByUserId = request.RegisteredByUserId,
            TemperatureC = request.TemperatureC,
            HumidityPct = request.HumidityPct,
            CoPpm = request.CoPpm,
            HeatStressIndex = request.HeatStressIndex,
            MeasuredAt = DateTime.UtcNow
        };

        await _repo.AddAsync(e, ct);
        return ToDto(e);
    }

    private static EnvironmentalDataDto ToDto(EnvironmentalDataEntity e) => new(
        e.EnvironmentalDataId,
        e.TrainingSessionId,
        e.RegisteredByUserId,
        e.TemperatureC,
        e.HumidityPct,
        e.CoPpm,
        e.HeatStressIndex,
        e.MeasuredAt);
}