using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
{
    public void Configure(EntityTypeBuilder<UserCredential> builder)
    {
        builder.ToTable("UserCredential");
        builder.HasKey(e => e.UserCredentialId);
        builder.Property(e => e.UserCredentialId).HasColumnName("user_credential_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(500).IsRequired();
        builder.Property(e => e.HashAlgorithm).HasColumnName("hash_algorithm").HasMaxLength(50).IsRequired();
        builder.Property(e => e.MfaEnabled).HasColumnName("mfa_enabled");
        builder.Property(e => e.FailedAttempts).HasColumnName("failed_attempts");
        builder.Property(e => e.LockedUntil).HasColumnName("locked_until");
        builder.Property(e => e.LastPasswordChangeAt).HasColumnName("last_password_change_at");
        builder.HasIndex(e => e.UserId).IsUnique();
    }
}
