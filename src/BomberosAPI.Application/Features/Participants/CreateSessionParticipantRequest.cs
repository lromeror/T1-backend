namespace BomberosAPI.Application.Features.Participants;

public record CreateSessionParticipantRequest(
    Guid TrainingSessionId,
    Guid TraineeFirefighterId,
    Guid? InvitationId
);