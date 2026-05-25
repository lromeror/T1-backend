namespace BomberosAPI.Domain.Entities;

public class MedicalHistory
{
    public Guid MedicalHistoryId { get; set; }
    public Guid TraineeFirefighterId { get; set; }
    public Guid CreatedByHealthPersonnelId { get; set; }
    public string? Allergies { get; set; }
    public string? PreexistingConditions { get; set; }
    public string? CurrentMedication { get; set; }
    public string? GeneralObservations { get; set; }
    public DateTime UpdatedAt { get; set; }
}
