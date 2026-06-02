using FluentValidation;

namespace BomberosAPI.Application.Features.Participants;

public class CreateSessionParticipantValidator : AbstractValidator<CreateSessionParticipantRequest>
{
    public CreateSessionParticipantValidator()
    {
        RuleFor(x => x.TrainingSessionId).NotEmpty();
        RuleFor(x => x.TraineeFirefighterId).NotEmpty();
    }
}