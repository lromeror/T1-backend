namespace BomberosAPI.Application.Features.Institutions;

public record InstitutionDto(
    Guid InstitutionId,
    string Name,
    string? Acronym,
    string? Country,
    string? City,
    bool IsActive
);
