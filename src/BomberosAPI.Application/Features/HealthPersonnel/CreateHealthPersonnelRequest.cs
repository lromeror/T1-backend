namespace BomberosAPI.Application.Features.HealthPersonnel;

public record CreateHealthPersonnelRequest(
    Guid UserId,
    string? Profession,
    string? Specialty,
    string? LicenseNumber,
    bool CanApproveDischarges
);