namespace BomberosAPI.Application.Features.MedicalHistory;

public record MedicalHistoryDto(
    Guid MedicalHistoryId,
    Guid TraineeFirefighterId,
    Guid CreatedByHealthPersonnelId,
    string? Allergies,
    string? PreexistingConditions,
    string? CurrentMedication,
    string? GeneralObservations,
    DateTime UpdatedAt
);