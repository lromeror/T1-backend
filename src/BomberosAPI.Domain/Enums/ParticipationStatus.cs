namespace BomberosAPI.Domain.Enums;
/// <summary>
/// Firefighter apprentice's participation status in a session. 
/// Defines the various states that a firefighter apprentice's participation in a training session can be in,
/// such as Invited, Confirmed, CheckedIn, Completed, NoShow, and Withdrawn.
/// </summary>
public enum ParticipationStatus
{
    Invited,
    Confirmed,
    CheckedIn,
    Completed,
    NoShow,
    Withdrawn
}
