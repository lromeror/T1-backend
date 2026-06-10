namespace BomberosAPI.Application.Features.EnvironmentalData;

public record CreateEnvironmentalDataRequest(
    Guid TrainingSessionId,
    Guid RegisteredByUserId,
    decimal? TemperatureC,
    decimal? HumidityPct,
    decimal? CoPpm,
    decimal? HeatStressIndex
);