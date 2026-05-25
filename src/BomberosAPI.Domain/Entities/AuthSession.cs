namespace BomberosAPI.Domain.Entities;

public class AuthSession
{
    public Guid AuthSessionId { get; set; }
    public Guid UserId { get; set; }
    public string RefreshTokenHash { get; set; } = null!;
    public string? Device { get; set; }
    public string? Ip { get; set; }
    public string? UserAgent { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}
