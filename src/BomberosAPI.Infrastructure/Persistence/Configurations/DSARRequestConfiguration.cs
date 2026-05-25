using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class DSARRequestConfiguration : IEntityTypeConfiguration<DSARRequest>
{
    public void Configure(EntityTypeBuilder<DSARRequest> builder)
    {
        builder.ToTable("DSARRequest");
        builder.HasKey(e => e.DSARRequestId);
        builder.Property(e => e.DSARRequestId).HasColumnName("dsar_request_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.ManagedByUserId).HasColumnName("managed_by_user_id");
        builder.Property(e => e.RightType).HasColumnName("right_type").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Response).HasColumnName("response");
        builder.Property(e => e.RequestedAt).HasColumnName("requested_at");
        builder.Property(e => e.RespondedAt).HasColumnName("responded_at");
        builder.Property(e => e.LegalDeadlineAt).HasColumnName("legal_deadline_at");
    }
}
