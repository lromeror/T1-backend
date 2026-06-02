using BomberosAPI.Application.Common.Exceptions;
using BomberosAPI.Application.Common.Interfaces;
using BomberosAPI.Application.Features.Participants;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using FluentValidation;
using AppValidationException = BomberosAPI.Application.Common.Exceptions.ValidationException;

namespace BomberosAPI.Application.Features.Invitations;

public class InvitationService
{
    private readonly IInvitationRepository _repo;
    private readonly ISessionParticipantRepository _participantRepo;
    private readonly IPasswordHasher _hasher;
    private readonly IValidator<CreateInvitationRequest> _createValidator;
    private readonly ICurrentUserService _currentUser;

    public InvitationService(
        IInvitationRepository repo,
        ISessionParticipantRepository participantRepo,
        IPasswordHasher hasher,
        IValidator<CreateInvitationRequest> createValidator,
        ICurrentUserService currentUser)
    {
        _repo = repo;
        _participantRepo = participantRepo;
        _hasher = hasher;
        _createValidator = createValidator;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<InvitationDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<InvitationDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var invitation = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Invitation", id);
        return ToDto(invitation);
    }

    public async Task<CreateInvitationResponse> CreateAsync(CreateInvitationRequest request, CancellationToken ct = default)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new AppValidationException(errors);
        }

        // Generate a random token (UUID-based, unique and unguessable)
        var plainToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        var tokenHash = _hasher.Hash(plainToken);

        var invitation = new Invitation
        {
            InvitationId = Guid.NewGuid(),
            SenderUserId = _currentUser.IsAuthenticated
    ? _currentUser.UserId
    : throw new UnauthorizedAccessException("No authenticated user."),
            TargetUserId = request.TargetUserId,
            TrainingSessionId = request.TrainingSessionId,
            TargetRoleId = request.TargetRoleId,
            TargetEmail = request.TargetEmail,
            InvitationTokenHash = tokenHash,
            Status = "Pending",
            ExpiresAt = request.ExpiresAt,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(invitation, ct);

        // Return the plain token ONCE — never stored, never retrievable again
        return new CreateInvitationResponse(ToDto(invitation), plainToken);
    }

    public async Task<SessionParticipantDto?> AcceptAsync(Guid id, CancellationToken ct = default)
    {
        var invitation = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Invitation", id);

        EnsurePending(invitation);
        EnsureNotExpired(invitation);

        invitation.Status = "Accepted";
        invitation.RespondedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(invitation, ct);

        // If the invitation was for a specific session AND we have a target user → create Participant
        if (invitation.TrainingSessionId is null || invitation.TargetUserId is null)
            return null;

        var participant = new SessionParticipant
        {
            SessionParticipantId = Guid.NewGuid(),
            TrainingSessionId = invitation.TrainingSessionId.Value,
            TraineeFirefighterId = invitation.TargetUserId.Value,
            InvitationId = invitation.InvitationId,
            ParticipationStatus = "Confirmed",
            AttendanceConfirmed = false
        };

        await _participantRepo.AddAsync(participant, ct);

        return new SessionParticipantDto(
            participant.SessionParticipantId,
            participant.TrainingSessionId,
            participant.TraineeFirefighterId,
            participant.InvitationId,
            participant.ParticipationStatus,
            participant.AttendanceConfirmed,
            participant.CheckInAt,
            participant.Observations);
    }

    public async Task RejectAsync(Guid id, CancellationToken ct = default)
    {
        var invitation = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Invitation", id);

        EnsurePending(invitation);

        invitation.Status = "Rejected";
        invitation.RespondedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(invitation, ct);
    }

    public async Task RevokeAsync(Guid id, CancellationToken ct = default)
    {
        var invitation = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Invitation", id);

        EnsurePending(invitation);

        invitation.Status = "Revoked";
        invitation.RespondedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(invitation, ct);
    }

    private static void EnsurePending(Invitation invitation)
    {
        if (invitation.Status != "Pending")
            throw new ConflictException($"Invitation is not pending (current status: {invitation.Status}).");
    }

    private static void EnsureNotExpired(Invitation invitation)
    {
        if (invitation.ExpiresAt < DateTime.UtcNow)
            throw new ConflictException("Invitation has expired.");
    }

    private static InvitationDto ToDto(Invitation i) => new(
        i.InvitationId,
        i.SenderUserId,
        i.TargetUserId,
        i.TrainingSessionId,
        i.TargetRoleId,
        i.TargetEmail,
        i.Status,
        i.ExpiresAt,
        i.RespondedAt,
        i.CreatedAt);
}