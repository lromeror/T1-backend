namespace BomberosAPI.Domain.Entities;

public class CriticalAlert
{
    public Guid CriticalAlertId { get; set; }
    public Guid SessionParticipantId { get; set; }
    public Guid? VitalSignsMeasurementId { get; set; }
    public Guid? SymptomReportId { get; set; }
    public Guid? EnvironmentalDataId { get; set; }
    public Guid? AttendedByUserId { get; set; }
    public string AlertType { get; set; } = null!;
    public string Severity { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime? AttendedAt { get; set; }
}
