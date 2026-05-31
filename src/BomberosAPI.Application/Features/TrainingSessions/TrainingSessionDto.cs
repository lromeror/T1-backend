namespace BomberosAPI.Application.Features.TrainingSessions;

public record TrainingSessionDto(
    Guid TrainingSessionId,
    Guid InstitutionId,
    Guid TrainingLocationId,
    Guid CreatedByUserId,
    string? SessionCode,
    string Title,
    string? Description,
    string Status,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    DateTime? ActualStart,
    DateTime? ActualEnd,
    int? PlannedCapacity
);
