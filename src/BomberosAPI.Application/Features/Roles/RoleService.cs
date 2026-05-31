using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;

namespace BomberosAPI.Application.Features.Roles;

public class RoleService
{
    private readonly IRoleRepository _repo;

    public RoleService(IRoleRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<RoleDto>> GetAllAsync(CancellationToken ct = default)
    {
        var roles = await _repo.GetAllAsync(ct);
        return roles.Select(ToDto).ToList();
    }

    private static RoleDto ToDto(Role r) => new(r.RoleId, r.Code, r.Name, r.Description);
}
