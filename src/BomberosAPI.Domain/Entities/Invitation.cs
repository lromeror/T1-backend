namespace BomberosAPI.Domain.Entities;

public class Invitation
{
    public Guid InvitationId { get; set; }
    public Guid SenderUserId { get; set; }
    public Guid? TargetUserId { get; set; }
    public Guid? TrainingSessionId { get; set; }
    public Guid? TargetRoleId { get; set; }
    public string TargetEmail { get; set; } = null!;
    public string InvitationTokenHash { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
