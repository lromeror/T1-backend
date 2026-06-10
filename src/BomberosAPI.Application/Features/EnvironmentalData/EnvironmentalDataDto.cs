namespace BomberosAPI.Application.Features.EnvironmentalData;

public record EnvironmentalDataDto(
    Guid EnvironmentalDataId,
    Guid TrainingSessionId,
    Guid RegisteredByUserId,
    decimal? TemperatureC,
    decimal? HumidityPct,
    decimal? CoPpm,
    decimal? HeatStressIndex,
    DateTime MeasuredAt
);