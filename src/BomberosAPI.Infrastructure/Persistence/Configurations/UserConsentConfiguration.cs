using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class UserConsentConfiguration : IEntityTypeConfiguration<UserConsent>
{
    public void Configure(EntityTypeBuilder<UserConsent> builder)
    {
        builder.ToTable("UserConsent");
        builder.HasKey(e => e.UserConsentId);
        builder.Property(e => e.UserConsentId).HasColumnName("user_consent_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.InstitutionId).HasColumnName("institution_id");
        builder.Property(e => e.ConsentDocumentId).HasColumnName("consent_document_id");
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        builder.Property(e => e.GrantedAt).HasColumnName("granted_at");
        builder.Property(e => e.RevokedAt).HasColumnName("revoked_at");
        builder.Property(e => e.ExpiresAt).HasColumnName("expires_at");
    }
}
