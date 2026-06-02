using FluentValidation;

namespace BomberosAPI.Application.Features.EnvironmentalData;

public class CreateEnvironmentalDataValidator : AbstractValidator<CreateEnvironmentalDataRequest>
{
    public CreateEnvironmentalDataValidator()
    {
        RuleFor(x => x.TrainingSessionId).NotEmpty();
        RuleFor(x => x.RegisteredByUserId).NotEmpty();

        RuleFor(x => x.TemperatureC)
            .InclusiveBetween(-30, 70).When(x => x.TemperatureC.HasValue)
            .WithMessage("TemperatureC must be between -30 and 70 °C.");

        RuleFor(x => x.HumidityPct)
            .InclusiveBetween(0, 100).When(x => x.HumidityPct.HasValue)
            .WithMessage("HumidityPct must be between 0 and 100 %.");

        RuleFor(x => x.CoPpm)
            .GreaterThanOrEqualTo(0).When(x => x.CoPpm.HasValue)
            .WithMessage("CoPpm must be non-negative.");

        RuleFor(x => x.HeatStressIndex)
            .GreaterThanOrEqualTo(0).When(x => x.HeatStressIndex.HasValue)
            .WithMessage("HeatStressIndex must be non-negative.");
    }
}