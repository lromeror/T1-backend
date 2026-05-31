using BomberosAPI.Application.Common.Interfaces;

namespace BomberosAPI.Infrastructure.Services;

/// <summary>
/// Implementacion de hashing usando BCrypt con work factor 12.
/// Resistente a ataques de fuerza bruta y rainbow tables.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool Verify(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}
