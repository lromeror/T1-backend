namespace BomberosAPI.Application.Features.TrainingSessions;

public record UpdateTrainingSessionRequest(
    string Title,
    string? Description,
    DateTime ScheduledStart,
    DateTime ScheduledEnd,
    int? PlannedCapacity
);
