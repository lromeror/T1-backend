using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class AuthSessionConfiguration : IEntityTypeConfiguration<AuthSession>
{
    public void Configure(EntityTypeBuilder<AuthSession> builder)
    {
        builder.ToTable("AuthSession");
        builder.HasKey(e => e.AuthSessionId);
        builder.Property(e => e.AuthSessionId).HasColumnName("auth_session_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.RefreshTokenHash).HasColumnName("refresh_token_hash").HasMaxLength(500).IsRequired();
        builder.Property(e => e.Device).HasColumnName("device").HasMaxLength(200);
        builder.Property(e => e.Ip).HasColumnName("ip").HasMaxLength(45);
        builder.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        builder.Property(e => e.ClosedAt).HasColumnName("closed_at");
    }
}
