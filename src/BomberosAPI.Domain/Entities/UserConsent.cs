namespace BomberosAPI.Domain.Entities;

public class UserConsent
{
    public Guid UserConsentId { get; set; }
    public Guid UserId { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid ConsentDocumentId { get; set; }
    public string Status { get; set; } = "active";
    public DateTime GrantedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
