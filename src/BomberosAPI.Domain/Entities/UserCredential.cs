namespace BomberosAPI.Domain.Entities;

public class UserCredential
{
    public Guid UserCredentialId { get; set; }
    public Guid UserId { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string HashAlgorithm { get; set; } = "bcrypt";
    public bool MfaEnabled { get; set; } = false;
    public int FailedAttempts { get; set; } = 0;
    public DateTime? LockedUntil { get; set; }
    public DateTime? LastPasswordChangeAt { get; set; }
}
