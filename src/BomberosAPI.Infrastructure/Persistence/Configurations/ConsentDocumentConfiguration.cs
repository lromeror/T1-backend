using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class ConsentDocumentConfiguration : IEntityTypeConfiguration<ConsentDocument>
{
    public void Configure(EntityTypeBuilder<ConsentDocument> builder)
    {
        builder.ToTable("ConsentDocument");
        builder.HasKey(e => e.ConsentDocumentId);
        builder.Property(e => e.ConsentDocumentId).HasColumnName("consent_document_id");
        builder.Property(e => e.ConsentType).HasColumnName("consent_type").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Version).HasColumnName("version").HasMaxLength(20).IsRequired();
        builder.Property(e => e.TextContent).HasColumnName("text_content");
        builder.Property(e => e.ValidFrom).HasColumnName("valid_from");
        builder.Property(e => e.ValidUntil).HasColumnName("valid_until");
    }
}
