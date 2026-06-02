using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentValidation;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.Participants;

public class SessionParticipantService
{
    private readonly ISessionParticipantRepository _repo;
    private readonly ITrainingSessionRepository _sessionRepo;
    private readonly ITraineeFirefighterRepository _traineeRepo;
    private readonly IValidator<CreateSessionParticipantRequest> _createValidator;

    public SessionParticipantService(
        ISessionParticipantRepository repo,
        ITrainingSessionRepository sessionRepo,
        ITraineeFirefighterRepository traineeRepo,
        IValidator<CreateSessionParticipantRequest> createValidator)
    {
        _repo = repo;
        _sessionRepo = sessionRepo;
        _traineeRepo = traineeRepo;
        _createValidator = createValidator;
    }

    public async Task<IReadOnlyList<SessionParticipantDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<SessionParticipantDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var participant = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("SessionParticipant", id);
        return ToDto(participant);
    }

    public async Task<SessionParticipantDto> CreateAsync(CreateSessionParticipantRequest request, CancellationToken ct = default)
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

        if (await _traineeRepo.GetByIdAsync(request.TraineeFirefighterId, ct) is null)
            throw new NotFoundException("TraineeFirefighter", request.TraineeFirefighterId);

        var participant = new SessionParticipant
        {
            SessionParticipantId = Guid.NewGuid(),
            TrainingSessionId = request.TrainingSessionId,
            TraineeFirefighterId = request.TraineeFirefighterId,
            InvitationId = request.InvitationId,
            ParticipationStatus = "Invited",
            AttendanceConfirmed = false
        };

        await _repo.AddAsync(participant, ct);
        return ToDto(participant);
    }

    public async Task CheckInAsync(Guid id, CancellationToken ct = default)
    {
        var participant = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("SessionParticipant", id);

        participant.ParticipationStatus = "CheckedIn";
        participant.AttendanceConfirmed = true;
        participant.CheckInAt = DateTime.UtcNow;

        await _repo.UpdateAsync(participant, ct);
    }

    private static SessionParticipantDto ToDto(SessionParticipant p) => new(
        p.SessionParticipantId,
        p.TrainingSessionId,
        p.TraineeFirefighterId,
        p.InvitationId,
        p.ParticipationStatus,
        p.AttendanceConfirmed,
        p.CheckInAt,
        p.Observations);
}