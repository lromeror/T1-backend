using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class OfflineSyncQueueConfiguration : IEntityTypeConfiguration<OfflineSyncQueue>
{
    public void Configure(EntityTypeBuilder<OfflineSyncQueue> builder)
    {
        builder.ToTable("OfflineSyncQueue");
        builder.HasKey(e => e.OfflineSyncQueueId);
        builder.Property(e => e.OfflineSyncQueueId).HasColumnName("offline_sync_queue_id");
        builder.Property(e => e.SourceTable).HasColumnName("source_table").HasMaxLength(100).IsRequired();
        builder.Property(e => e.RecordId).HasColumnName("record_id");
        builder.Property(e => e.PayloadJson).HasColumnName("payload_json").IsRequired();
        builder.Property(e => e.Attempts).HasColumnName("attempts");
        builder.Property(e => e.MaxAttempts).HasColumnName("max_attempts");
        builder.Property(e => e.Synced).HasColumnName("synced");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.SyncedAt).HasColumnName("synced_at");
    }
}
