namespace BomberosAPI.Domain.Entities;

public class TraineeFirefighter
{
    public Guid TraineeFirefighterId { get; set; }
    public Guid UserId { get; set; }
    public string? ApplicantCode { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Sex { get; set; }
    public string? BloodType { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? TrainingStatus { get; set; }
}
