namespace BomberosAPI.Application.Features.TraineeFirefighters;

public record CreateTraineeFirefighterRequest(
    Guid UserId,
    string ApplicantCode,
    DateOnly BirthDate,
    string Sex,
    string? BloodType,
    string? EmergencyContactName,
    string? EmergencyContactPhone
);