using FluentValidation;

namespace BomberosAPI.Application.Features.HealthPersonnel;

public class CreateHealthPersonnelValidator : AbstractValidator<CreateHealthPersonnelRequest>
{
    public CreateHealthPersonnelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Profession).MaximumLength(100);
        RuleFor(x => x.Specialty).MaximumLength(100);
        RuleFor(x => x.LicenseNumber).MaximumLength(50);
    }
}