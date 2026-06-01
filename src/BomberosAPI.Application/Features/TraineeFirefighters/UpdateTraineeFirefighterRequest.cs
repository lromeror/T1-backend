namespace BomberosAPI.Application.Features.TraineeFirefighters;

public record UpdateTraineeFirefighterRequest(
    string? BloodType,
    string? EmergencyContactName,
    string? EmergencyContactPhone
);