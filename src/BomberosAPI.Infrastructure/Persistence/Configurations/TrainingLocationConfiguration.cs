using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class TrainingLocationConfiguration : IEntityTypeConfiguration<TrainingLocation>
{
    public void Configure(EntityTypeBuilder<TrainingLocation> builder)
    {
        builder.ToTable("TrainingLocation");
        builder.HasKey(e => e.TrainingLocationId);
        builder.Property(e => e.TrainingLocationId).HasColumnName("training_location_id");
        builder.Property(e => e.InstitutionId).HasColumnName("institution_id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(e => e.LocationType).HasColumnName("location_type").HasMaxLength(100);
        builder.Property(e => e.Address).HasColumnName("address").HasMaxLength(300);
        builder.Property(e => e.MaxCapacity).HasColumnName("max_capacity");
    }
}
