namespace BomberosAPI.Application.Features.Users;

public record UserDto(
    Guid UserId,
    Guid InstitutionId,
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string AccountStatus,
    DateTime CreatedAt
);
