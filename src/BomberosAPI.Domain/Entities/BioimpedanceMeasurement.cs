namespace BomberosAPI.Domain.Entities;

public class BioimpedanceMeasurement
{
    public Guid BioimpedanceMeasurementId { get; set; }
    public Guid SessionParticipantId { get; set; }
    public Guid RegisteredByHealthPersonnelId { get; set; }
    public decimal? WeightKg { get; set; }
    public decimal? FatPercentage { get; set; }
    public decimal? MuscleMassKg { get; set; }
    public decimal? BodyWaterPct { get; set; }
    public decimal? BasalMetabolicRate { get; set; }
    public DateTime TakenAt { get; set; }
}
