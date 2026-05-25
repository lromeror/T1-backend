namespace BomberosAPI.Domain.Entities;

public class ChangeAudit
{
    public Guid ChangeAuditId { get; set; }
    public Guid ActorUserId { get; set; }
    public string Entity { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string Operation { get; set; } = null!;
    public string? PreviousValuesJson { get; set; }
    public string? NewValuesJson { get; set; }
    public DateTime OccurredAt { get; set; }
}
