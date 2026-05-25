using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class SessionResultConfiguration : IEntityTypeConfiguration<SessionResult>
{
    public void Configure(EntityTypeBuilder<SessionResult> builder)
    {
        builder.ToTable("SessionResult");
        builder.HasKey(e => e.SessionResultId);
        builder.Property(e => e.SessionResultId).HasColumnName("session_result_id");
        builder.Property(e => e.SessionParticipantId).HasColumnName("session_participant_id");
        builder.Property(e => e.ValidatedByUserId).HasColumnName("validated_by_user_id");
        builder.Property(e => e.PerformanceScore).HasColumnName("performance_score").HasPrecision(5, 2);
        builder.Property(e => e.RiskClassification).HasColumnName("risk_classification").HasMaxLength(50);
        builder.Property(e => e.FitToContinue).HasColumnName("fit_to_continue");
        builder.Property(e => e.Summary).HasColumnName("summary");
        builder.Property(e => e.GeneratedAt).HasColumnName("generated_at");
    }
}
