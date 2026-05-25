using BomberosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BomberosAPI.Infrastructure.Persistence.Configurations;

public class SessionParticipantConfiguration : IEntityTypeConfiguration<SessionParticipant>
{
    public void Configure(EntityTypeBuilder<SessionParticipant> builder)
    {
        builder.ToTable("SessionParticipant");
        builder.HasKey(e => e.SessionParticipantId);
        builder.Property(e => e.SessionParticipantId).HasColumnName("session_participant_id");
        builder.Property(e => e.TrainingSessionId).HasColumnName("training_session_id");
        builder.Property(e => e.TraineeFirefighterId).HasColumnName("trainee_firefighter_id");
        builder.Property(e => e.InvitationId).HasColumnName("invitation_id");
        builder.Property(e => e.ParticipationStatus).HasColumnName("participation_status").HasMaxLength(50);
        builder.Property(e => e.AttendanceConfirmed).HasColumnName("attendance_confirmed");
        builder.Property(e => e.CheckInAt).HasColumnName("check_in_at");
        builder.Property(e => e.Observations).HasColumnName("observations");
    }
}
