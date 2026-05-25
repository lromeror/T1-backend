using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class CriticalAlertConfiguration : IEntityTypeConfiguration<CriticalAlert>
{
    public void Configure(EntityTypeBuilder<CriticalAlert> builder)
    {
        builder.ToTable("CriticalAlert");
        builder.HasKey(e => e.CriticalAlertId);
        builder.Property(e => e.CriticalAlertId).HasColumnName("critical_alert_id");
        builder.Property(e => e.SessionParticipantId).HasColumnName("session_participant_id");
        builder.Property(e => e.VitalSignsMeasurementId).HasColumnName("vital_signs_measurement_id");
        builder.Property(e => e.SymptomReportId).HasColumnName("symptom_report_id");
        builder.Property(e => e.EnvironmentalDataId).HasColumnName("environmental_data_id");
        builder.Property(e => e.AttendedByUserId).HasColumnName("attended_by_user_id");
        builder.Property(e => e.AlertType).HasColumnName("alert_type").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Severity).HasColumnName("severity").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.GeneratedAt).HasColumnName("generated_at");
        builder.Property(e => e.AttendedAt).HasColumnName("attended_at");
    }
}
