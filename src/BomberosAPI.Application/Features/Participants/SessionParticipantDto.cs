namespace BomberosAPI.Application.Features.Participants;

public record SessionParticipantDto(
    Guid SessionParticipantId,
    Guid TrainingSessionId,
    Guid TraineeFirefighterId,
    Guid? InvitationId,
    string? ParticipationStatus,
    bool? AttendanceConfirmed,
    DateTime? CheckinAt,
    string? Observations
);