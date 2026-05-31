using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Application.Common.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user, IReadOnlyList<string> roles);
}
