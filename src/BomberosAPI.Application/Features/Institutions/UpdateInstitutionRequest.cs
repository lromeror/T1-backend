namespace BomberosAPI.Application.Features.Institutions;

public record UpdateInstitutionRequest(
    string Name,
    string? Acronym,
    string? Country,
    string? City
);
