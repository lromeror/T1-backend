namespace BomberosAPI.Domain.Entities;

public class OfflineSyncQueue
{
    public Guid OfflineSyncQueueId { get; set; }
    public string SourceTable { get; set; } = null!;
    public Guid RecordId { get; set; }
    public string PayloadJson { get; set; } = null!;
    public int Attempts { get; set; } = 0;
    public int MaxAttempts { get; set; } = 3;
    public bool Synced { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? SyncedAt { get; set; }
}
