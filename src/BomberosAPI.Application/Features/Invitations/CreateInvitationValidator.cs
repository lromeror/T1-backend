using FluentValidation;

namespace BomberosAPI.Application.Features.Invitations;

public class CreateInvitationValidator : AbstractValidator<CreateInvitationRequest>
{
    public CreateInvitationValidator()
    {
        RuleFor(x => x.TargetEmail).NotEmpty().EmailAddress().MaximumLength(254);
        RuleFor(x => x.ExpiresAt).NotEmpty()
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("ExpiresAt must be a future date.");
    }
}