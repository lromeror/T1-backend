namespace BomberosAPI.Domain.Entities;

public class TrainingLocation
{
    public Guid TrainingLocationId { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; } = null!;
    public string? LocationType { get; set; }
    public string? Address { get; set; }
    public int? MaxCapacity { get; set; }
}
