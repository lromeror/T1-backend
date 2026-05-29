using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Repository for participants enrolled in training sessions.
/// </summary>
public interface ISessionParticipantRepository : IRepository<SessionParticipant>
{
  
    /// Lists the participants of a specific session.
    Task<IEnumerable<SessionParticipant>> GetBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default);

    /// Lists the participations of a trainee across all sessions.
    Task<IEnumerable<SessionParticipant>> GetByTraineeAsync(Guid traineeId, CancellationToken cancellationToken = default);
}