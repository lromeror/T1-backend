namespace BomberosAPI.Application.Features.TrainingLocations;

public record TrainingLocationDto(
    Guid   TrainingLocationId,
    Guid   InstitutionId,
    string Name,
    string? LocationType,
    string? Address,
    int?   MaxCapacity
);
