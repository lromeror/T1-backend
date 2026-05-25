namespace BomberosAPI.Domain.Entities;

public class TrainingSession
{
    public Guid TrainingSessionId { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid TrainingLocationId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string? SessionCode { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public DateTime ScheduledStart { get; set; }
    public DateTime ScheduledEnd { get; set; }
    public DateTime? ActualStart { get; set; }
    public DateTime? ActualEnd { get; set; }
    public int? PlannedCapacity { get; set; }
}
