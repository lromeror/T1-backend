using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Enums;

namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Repository for invitations to the system or to specific sessions.
/// </summary>
public interface IInvitationRepository : IRepository<Invitation>
{
 
    /// Lists the invitations filtered by their current status.
    Task<IEnumerable<Invitation>> GetByStatusAsync(InvitationStatus status, CancellationToken cancellationToken = default);

    /// Finds an invitation by the hash of its token (used to validate acceptance).
    Task<Invitation?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);

    /// Lists the pending invitations of a specific session.
    Task<IEnumerable<Invitation>> GetPendingBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
}