using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class TrainingSessionConfiguration : IEntityTypeConfiguration<TrainingSession>
{
    public void Configure(EntityTypeBuilder<TrainingSession> builder)
    {
        builder.ToTable("TrainingSession");
        builder.HasKey(e => e.TrainingSessionId);
        builder.Property(e => e.TrainingSessionId).HasColumnName("training_session_id");
        builder.Property(e => e.InstitutionId).HasColumnName("institution_id");
        builder.Property(e => e.TrainingLocationId).HasColumnName("training_location_id");
        builder.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
        builder.Property(e => e.SessionCode).HasColumnName("session_code").HasMaxLength(50);
        builder.Property(e => e.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ScheduledStart).HasColumnName("scheduled_start");
        builder.Property(e => e.ScheduledEnd).HasColumnName("scheduled_end");
        builder.Property(e => e.ActualStart).HasColumnName("actual_start");
        builder.Property(e => e.ActualEnd).HasColumnName("actual_end");
        builder.Property(e => e.PlannedCapacity).HasColumnName("planned_capacity");
    }
}
