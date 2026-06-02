using FluentValidation;

namespace BomberosAPI.Application.Features.MedicalHistory;

public class CreateMedicalHistoryValidator : AbstractValidator<CreateMedicalHistoryRequest>
{
    public CreateMedicalHistoryValidator()
    {
        RuleFor(x => x.TraineeFirefighterId).NotEmpty();
        RuleFor(x => x.CreatedByHealthPersonnelId).NotEmpty();
        RuleFor(x => x.Allergies).MaximumLength(1000);
        RuleFor(x => x.PreexistingConditions).MaximumLength(2000);
        RuleFor(x => x.CurrentMedication).MaximumLength(1000);
        RuleFor(x => x.GeneralObservations).MaximumLength(2000);
    }
}