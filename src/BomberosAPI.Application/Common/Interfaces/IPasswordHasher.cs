namespace BomberosAPI.Application.Common.Interfaces;

public interface IPasswordHasher
{
    /// <summary>Genera un hash seguro de la contraseña en texto plano.</summary>
    string Hash(string password);

    /// <summary>Verifica si la contraseña en texto plano coincide con el hash almacenado.</summary>
    bool Verify(string password, string hash);
}
