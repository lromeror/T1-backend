namespace BomberosAPI.Domain.Entities;

public class SymptomReport
{
    public Guid SymptomReportId { get; set; }
    public Guid SessionParticipantId { get; set; }
    public Guid ReportedByUserId { get; set; }
    public string? Severity { get; set; }
    public string? Symptoms { get; set; }
    public bool RequiresAlert { get; set; } = false;
    public DateTime ReportedAt { get; set; }
}
