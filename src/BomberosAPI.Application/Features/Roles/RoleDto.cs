namespace BomberosAPI.Application.Features.Roles;

public record RoleDto(
    Guid RoleId,
    string Code,
    string Name,
    string? Description
);
