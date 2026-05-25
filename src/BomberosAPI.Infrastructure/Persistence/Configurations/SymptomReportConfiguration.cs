using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class SymptomReportConfiguration : IEntityTypeConfiguration<SymptomReport>
{
    public void Configure(EntityTypeBuilder<SymptomReport> builder)
    {
        builder.ToTable("SymptomReport");
        builder.HasKey(e => e.SymptomReportId);
        builder.Property(e => e.SymptomReportId).HasColumnName("symptom_report_id");
        builder.Property(e => e.SessionParticipantId).HasColumnName("session_participant_id");
        builder.Property(e => e.ReportedByUserId).HasColumnName("reported_by_user_id");
        builder.Property(e => e.Severity).HasColumnName("severity").HasMaxLength(50);
        builder.Property(e => e.Symptoms).HasColumnName("symptoms");
        builder.Property(e => e.RequiresAlert).HasColumnName("requires_alert");
        builder.Property(e => e.ReportedAt).HasColumnName("reported_at");
    }
}
