using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("PasswordResetToken");
        builder.HasKey(e => e.PasswordResetTokenId);
        builder.Property(e => e.PasswordResetTokenId).HasColumnName("password_reset_token_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.TokenHash).HasColumnName("token_hash").HasMaxLength(500).IsRequired();
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        builder.Property(e => e.UsedAt).HasColumnName("used_at");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
    }
}
