namespace BomberosAPI.Application.Features.MedicalHistory;

public record UpdateMedicalHistoryRequest(
    string? Allergies,
    string? PreexistingConditions,
    string? CurrentMedication,
    string? GeneralObservations
);