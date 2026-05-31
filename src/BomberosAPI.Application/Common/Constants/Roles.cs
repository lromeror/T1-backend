namespace BomberosAPI.Application.Common.Constants;

/// <summary>
/// Codigos de roles del dominio. Deben coincidir con Role.Code en la base de datos.
/// </summary>
public static class Roles
{
    public const string Admin           = "admin";
    public const string HealthPersonnel = "health_personnel";
    public const string Instructor      = "instructor";
    public const string Trainee         = "trainee";
}
