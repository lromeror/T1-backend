namespace BomberosAPI.Domain.Entities;

public class AccessAudit
{
    public Guid AccessAuditId { get; set; }
    public Guid UserId { get; set; }
    public Guid? AuthSessionId { get; set; }
    public string Event { get; set; } = null!;
    public string? Ip { get; set; }
    public string? UserAgent { get; set; }
    public bool Success { get; set; }
    public DateTime OccurredAt { get; set; }
}
