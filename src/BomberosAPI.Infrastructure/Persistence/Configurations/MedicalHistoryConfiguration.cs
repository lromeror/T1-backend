using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class MedicalHistoryConfiguration : IEntityTypeConfiguration<MedicalHistory>
{
    public void Configure(EntityTypeBuilder<MedicalHistory> builder)
    {
        builder.ToTable("MedicalHistory");
        builder.HasKey(e => e.MedicalHistoryId);
        builder.Property(e => e.MedicalHistoryId).HasColumnName("medical_history_id");
        builder.Property(e => e.TraineeFirefighterId).HasColumnName("trainee_firefighter_id");
        builder.Property(e => e.CreatedByHealthPersonnelId).HasColumnName("created_by_health_personnel_id");
        builder.Property(e => e.Allergies).HasColumnName("allergies");
        builder.Property(e => e.PreexistingConditions).HasColumnName("preexisting_conditions");
        builder.Property(e => e.CurrentMedication).HasColumnName("current_medication");
        builder.Property(e => e.GeneralObservations).HasColumnName("general_observations");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
    }
}
