namespace BomberosAPI.Application.Features.TraineeFirefighters;

public record TraineeFirefighterDto(
    Guid   TraineeFirefighterId,
    Guid   UserId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? ApplicantCode,
    DateOnly? BirthDate,
    string? Sex,
    string? BloodType,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? TrainingStatus
);