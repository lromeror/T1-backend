namespace BomberosAPI.Application.Features.Institutions;

public record CreateInstitutionRequest(
    string Name,
    string? Acronym,
    string? Country,
    string? City
);
