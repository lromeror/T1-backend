namespace BomberosAPI.Application.Features.Users;

public record CreateUserRequest(
    Guid InstitutionId,
    string Email,
    string FirstName,
    string LastName,
    string? Phone
);