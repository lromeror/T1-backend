namespace BomberosAPI.Domain.Entities;

public class DSARRequest
{
    public Guid DSARRequestId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ManagedByUserId { get; set; }
    public string RightType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Description { get; set; }
    public string? Response { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public DateTime? LegalDeadlineAt { get; set; }
}
