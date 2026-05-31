
namespace BomberosAPI.Domain.Enums;

/// <summary>
/// Defines the life cycle of a training session. 
/// Defines the various states that a training session can be in, such as 
/// Scheduled, InProgress, Finished, and Cancelled.
/// </summary>

public enum SessionStatus
{
    Scheduled,
    InProgress,
    Finished,
    Cancelled
}

