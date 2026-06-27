using FluentValidation;

namespace BomberosAPI.Application.Features.Auth;

public record ForgotPasswordRequest(string Email);

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Must be a valid email address.");
    }
}

public record ForgotPasswordResult(string ResetToken);
