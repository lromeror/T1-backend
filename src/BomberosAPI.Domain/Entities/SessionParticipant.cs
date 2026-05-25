namespace BomberosAPI.Domain.Entities;

public class SessionParticipant
{
    public Guid SessionParticipantId { get; set; }
    public Guid TrainingSessionId { get; set; }
    public Guid TraineeFirefighterId { get; set; }
    public Guid? InvitationId { get; set; }
    public string? ParticipationStatus { get; set; }
    public bool AttendanceConfirmed { get; set; } = false;
    public DateTime? CheckInAt { get; set; }
    public string? Observations { get; set; }
}
