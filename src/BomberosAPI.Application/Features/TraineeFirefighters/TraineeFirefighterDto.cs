namespace BomberosAPI.Application.Features.TraineeFirefighters;

public record TraineeFirefighterDto(
    Guid TraineeFirefighterId,
    Guid UserId,
    string? ApplicantCode,
    DateOnly? BirthDate,
    string? Sex,
    string? BloodType,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? TrainingStatus
);