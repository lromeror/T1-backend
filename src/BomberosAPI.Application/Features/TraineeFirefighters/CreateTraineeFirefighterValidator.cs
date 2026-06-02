using FluentValidation;

namespace BomberosAPI.Application.Features.TraineeFirefighters;

public class CreateTraineeFirefighterValidator : AbstractValidator<CreateTraineeFirefighterRequest>
{
    public CreateTraineeFirefighterValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ApplicantCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BirthDate).NotEmpty();
        RuleFor(x => x.Sex).NotEmpty().MaximumLength(10);
        RuleFor(x => x.BloodType).MaximumLength(5);
        RuleFor(x => x.EmergencyContactName).MaximumLength(150);
        RuleFor(x => x.EmergencyContactPhone).MaximumLength(30);
    }
}