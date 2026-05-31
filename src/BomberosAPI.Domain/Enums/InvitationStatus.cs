namespace BomberosAPI.Domain.Enums;
/// <summary>
/// Status of an invitation to a training session. 
/// Defines the various states that an invitation can be in, such as
/// Pending, Accepted, Rejected, Expired, and Revoked.
/// </summary>
public enum InvitationStatus
{
    Pending,
    Accepted,
    Rejected,
    Expired,
    Revoked
}
