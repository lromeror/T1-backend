namespace BomberosAPI.Domain.Entities;

public class SessionResult
{
    public Guid SessionResultId { get; set; }
    public Guid SessionParticipantId { get; set; }
    public Guid ValidatedByUserId { get; set; }
    public decimal? PerformanceScore { get; set; }
    public string? RiskClassification { get; set; }
    public bool FitToContinue { get; set; } = true;
    public string? Summary { get; set; }
    public DateTime GeneratedAt { get; set; }
}
