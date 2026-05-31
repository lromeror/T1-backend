using FluentValidation;

namespace BomberosAPI.Application.Features.TrainingSessions;

public class CreateTrainingSessionValidator : AbstractValidator<CreateTrainingSessionRequest>
{
    public CreateTrainingSessionValidator()
    {
        RuleFor(x => x.InstitutionId).NotEmpty();
        RuleFor(x => x.TrainingLocationId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SessionCode).MaximumLength(50).When(x => x.SessionCode is not null);
        RuleFor(x => x.ScheduledStart).NotEmpty();
        RuleFor(x => x.ScheduledEnd).NotEmpty()
            .GreaterThan(x => x.ScheduledStart).WithMessage("ScheduledEnd must be after ScheduledStart.");
        RuleFor(x => x.PlannedCapacity).GreaterThan(0).When(x => x.PlannedCapacity is not null);
    }
}
