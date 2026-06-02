namespace BomberosAPI.Application.Features.MedicalHistory;

public record CreateMedicalHistoryRequest(
    Guid TraineeFirefighterId,
    Guid CreatedByHealthPersonnelId,
    string? Allergies,
    string? PreexistingConditions,
    string? CurrentMedication,
    string? GeneralObservations
);