namespace BomberosAPI.Application.Features.Invitations;

public record InvitationDto(
    Guid InvitationId,
    Guid SenderUserId,
    Guid? TargetUserId,
    Guid? TrainingSessionId,
    Guid? TargetRoleId,
    string TargetEmail,
    string Status,
    DateTime ExpiresAt,
    DateTime? RespondedAt,
    DateTime CreatedAt
);