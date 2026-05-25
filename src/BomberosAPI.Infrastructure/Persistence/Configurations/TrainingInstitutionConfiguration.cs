using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class TrainingInstitutionConfiguration : IEntityTypeConfiguration<TrainingInstitution>
{
    public void Configure(EntityTypeBuilder<TrainingInstitution> builder)
    {
        builder.ToTable("TrainingInstitution");
        builder.HasKey(e => e.InstitutionId);
        builder.Property(e => e.InstitutionId).HasColumnName("institution_id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(e => e.Acronym).HasColumnName("acronym").HasMaxLength(20);
        builder.Property(e => e.Country).HasColumnName("country").HasMaxLength(100);
        builder.Property(e => e.City).HasColumnName("city").HasMaxLength(100);
        builder.Property(e => e.IsActive).HasColumnName("is_active");
    }
}
