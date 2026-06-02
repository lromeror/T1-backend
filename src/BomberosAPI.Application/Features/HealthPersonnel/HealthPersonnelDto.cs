namespace BomberosAPI.Application.Features.HealthPersonnel;

public record HealthPersonnelDto(
    Guid HealthPersonnelId,
    Guid UserId,
    string? Profession,
    string? Specialty,
    string? LicenseNumber,
    bool CanApproveDischarges
);