namespace BomberosAPI.Application.Features.TrainingSessions;

public record CreateTrainingSessionRequest(
    Guid InstitutionId,
    Guid TrainingLocationId,
    string? SessionCode,
    string Title,
    string? Description,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    int? PlannedCapacity
);
