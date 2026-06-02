namespace BomberosAPI.Application.Features.Invitations;

public record CreateInvitationRequest(
    Guid? TargetUserId,
    Guid? TrainingSessionId,
    Guid? TargetRoleId,
    string TargetEmail,
    DateTime ExpiresAt
);