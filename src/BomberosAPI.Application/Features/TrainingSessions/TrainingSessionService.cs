using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Common.Interfaces;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentValidation;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.TrainingSessions;

public class TrainingSessionService
{
    private readonly ITrainingSessionRepository _repo;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateTrainingSessionRequest> _createValidator;

    public TrainingSessionService(
        ITrainingSessionRepository repo,
        ICurrentUserService currentUser,
        IValidator<CreateTrainingSessionRequest> createValidator)
    {
        _repo = repo;
        _currentUser = currentUser;
        _createValidator = createValidator;
    }

    public async Task<IReadOnlyList<TrainingSessionDto>> GetAllAsync(CancellationToken ct = default)
    {
        var sessions = await _repo.GetAllAsync(ct);
        return sessions.Select(ToDto).ToList();
    }

    public async Task<TrainingSessionDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var session = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TrainingSession", id);
        return ToDto(session);
    }

    public async Task<TrainingSessionDto> CreateAsync(CreateTrainingSessionRequest request, CancellationToken ct = default)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }

        var session = new TrainingSession
        {
            TrainingSessionId = Guid.NewGuid(),
            InstitutionId = request.InstitutionId,
            TrainingLocationId = request.TrainingLocationId,
            CreatedByUserId = _currentUser.UserId,
            SessionCode = request.SessionCode,
            Title = request.Title,
            Description = request.Description,
            Status = "Scheduled",
            ScheduledStart = request.ScheduledStart,
            ScheduledEnd = request.ScheduledEnd,
            PlannedCapacity = request.PlannedCapacity
        };

        await _repo.AddAsync(session, ct);
        return ToDto(session);
    }

    public async Task<TrainingSessionDto> UpdateAsync(Guid id, UpdateTrainingSessionRequest request, CancellationToken ct = default)
    {
        var session = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TrainingSession", id);

        if (session.Status != "Scheduled")
            throw new BusinessRuleException("Only sessions with status 'Scheduled' can be updated.");

        session.Title = request.Title;
        session.Description = request.Description;
        session.ScheduledStart = request.ScheduledStart;
        session.ScheduledEnd = request.ScheduledEnd;
        session.PlannedCapacity = request.PlannedCapacity;

        await _repo.UpdateAsync(session, ct);
        return ToDto(session);
    }

    public async Task<TrainingSessionDto> SetStatusAsync(Guid id, string status, CancellationToken ct = default)
    {
        var session = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TrainingSession", id);

        if (status != "Cancelled")
            throw new BusinessRuleException("Only 'Cancelled' is accepted via this endpoint. Use /start or /finish for other transitions.");

        if (session.Status is "Finished" or "Cancelled")
            throw new BusinessRuleException($"Cannot cancel a session with status '{session.Status}'.");

        session.Status = "Cancelled";
        await _repo.UpdateAsync(session, ct);
        return ToDto(session);
    }

    public async Task<TrainingSessionDto> StartAsync(Guid id, CancellationToken ct = default)
    {
        var session = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TrainingSession", id);

        if (session.Status != "Scheduled")
            throw new BusinessRuleException($"Cannot start a session with status '{session.Status}'. Expected 'Scheduled'.");

        session.Status = "InProgress";
        session.ActualStart = DateTime.UtcNow;

        await _repo.UpdateAsync(session, ct);
        return ToDto(session);
    }

    public async Task<TrainingSessionDto> FinishAsync(Guid id, CancellationToken ct = default)
    {
        var session = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("TrainingSession", id);

        if (session.Status != "InProgress")
            throw new BusinessRuleException($"Cannot finish a session with status '{session.Status}'. Expected 'InProgress'.");

        session.Status = "Finished";
        session.ActualEnd = DateTime.UtcNow;

        await _repo.UpdateAsync(session, ct);
        return ToDto(session);
    }

    private static TrainingSessionDto ToDto(TrainingSession s) => new(
        s.TrainingSessionId,
        s.InstitutionId,
        s.TrainingLocationId,
        s.CreatedByUserId,
        s.SessionCode,
        s.Title,
        s.Description,
        s.Status,
        s.ScheduledStart,
        s.ScheduledEnd,
        s.ActualStart,
        s.ActualEnd,
        s.PlannedCapacity
    );
}
