using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class TraineeFirefighterConfiguration : IEntityTypeConfiguration<TraineeFirefighter>
{
    public void Configure(EntityTypeBuilder<TraineeFirefighter> builder)
    {
        builder.ToTable("TraineeFirefighter");
        builder.HasKey(e => e.TraineeFirefighterId);
        builder.Property(e => e.TraineeFirefighterId).HasColumnName("trainee_firefighter_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.ApplicantCode).HasColumnName("applicant_code").HasMaxLength(50);
        builder.Property(e => e.BirthDate).HasColumnName("birth_date");
        builder.Property(e => e.Sex).HasColumnName("sex").HasMaxLength(20);
        builder.Property(e => e.BloodType).HasColumnName("blood_type").HasMaxLength(10);
        builder.Property(e => e.EmergencyContactName).HasColumnName("emergency_contact_name").HasMaxLength(150);
        builder.Property(e => e.EmergencyContactPhone).HasColumnName("emergency_contact_phone").HasMaxLength(30);
        builder.Property(e => e.TrainingStatus).HasColumnName("training_status").HasMaxLength(50);
    }
}
