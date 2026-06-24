namespace BomberosAPI.Application.Common.Constants;

/// <summary>
/// Codigos de roles del dominio. Deben coincidir con Role.Code en la base de datos
/// y con los ROLES constants del frontend (uppercase snake_case).
/// </summary>
public static class Roles
{
    public const string SystemAdmin        = "SYSTEM_ADMIN";
    public const string Admin              = "ADMIN";
    public const string Medical            = "MEDICAL";
    public const string FirefighterTrainee = "FIREFIGHTER_TRAINEE";
    public const string Capacitator        = "CAPACITATOR";
    public const string Researcher         = "RESEARCHER";
    public const string FireChief          = "FIRE_CHIEF";
}
