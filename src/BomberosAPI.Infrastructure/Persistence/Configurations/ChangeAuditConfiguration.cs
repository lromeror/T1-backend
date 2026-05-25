using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class ChangeAuditConfiguration : IEntityTypeConfiguration<ChangeAudit>
{
    public void Configure(EntityTypeBuilder<ChangeAudit> builder)
    {
        builder.ToTable("ChangeAudit");
        builder.HasKey(e => e.ChangeAuditId);
        builder.Property(e => e.ChangeAuditId).HasColumnName("change_audit_id");
        builder.Property(e => e.ActorUserId).HasColumnName("actor_user_id");
        builder.Property(e => e.Entity).HasColumnName("entity").HasMaxLength(100).IsRequired();
        builder.Property(e => e.EntityId).HasColumnName("entity_id");
        builder.Property(e => e.Operation).HasColumnName("operation").HasMaxLength(50).IsRequired();
        builder.Property(e => e.PreviousValuesJson).HasColumnName("previous_values_json");
        builder.Property(e => e.NewValuesJson).HasColumnName("new_values_json");
        builder.Property(e => e.OccurredAt).HasColumnName("occurred_at");
    }
}
