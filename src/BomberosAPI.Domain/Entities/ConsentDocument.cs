namespace BomberosAPI.Domain.Entities;

public class ConsentDocument
{
    public Guid ConsentDocumentId { get; set; }
    public string ConsentType { get; set; } = null!;
    public string Version { get; set; } = null!;
    public string? TextContent { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
}
