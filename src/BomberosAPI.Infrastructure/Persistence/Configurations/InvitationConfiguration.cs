using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("Invitation");
        builder.HasKey(e => e.InvitationId);
        builder.Property(e => e.InvitationId).HasColumnName("invitation_id");
        builder.Property(e => e.SenderUserId).HasColumnName("sender_user_id");
        builder.Property(e => e.TargetUserId).HasColumnName("target_user_id");
        builder.Property(e => e.TrainingSessionId).HasColumnName("training_session_id");
        builder.Property(e => e.TargetRoleId).HasColumnName("target_role_id");
        builder.Property(e => e.TargetEmail).HasColumnName("target_email").HasMaxLength(254).IsRequired();
        builder.Property(e => e.InvitationTokenHash).HasColumnName("invitation_token_hash").HasMaxLength(500).IsRequired();
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        builder.Property(e => e.RespondedAt).HasColumnName("responded_at");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
    }
}
