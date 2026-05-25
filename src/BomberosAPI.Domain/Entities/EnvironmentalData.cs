namespace BomberosAPI.Domain.Entities;

public class EnvironmentalData
{
    public Guid EnvironmentalDataId { get; set; }
    public Guid TrainingSessionId { get; set; }
    public Guid RegisteredByUserId { get; set; }
    public decimal? TemperatureC { get; set; }
    public decimal? HumidityPct { get; set; }
    public decimal? CoPpm { get; set; }
    public decimal? HeatStressIndex { get; set; }
    public DateTime MeasuredAt { get; set; }
}
