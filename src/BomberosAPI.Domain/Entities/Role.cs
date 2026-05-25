namespace BomberosAPI.Domain.Entities;

public class Role
{
    public Guid RoleId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
