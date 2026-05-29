
namespace BomberosAPI.Domain.Enums;

/// <summary>
/// Estado de atencion de una alerta critica.
/// status of a critical alert. 
/// Defines the various states that a critical alert can be in, 
/// such as Open, Acknowledged, InProgress, Resolved, and Dismissed.
/// </summary>
public enum AlertStatus
{
    Open,
    Acknowledged,
    InProgress,
    Resolved,
    Dismissed
}
