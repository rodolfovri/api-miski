using FluentValidation;

namespace Miski.Application.Features.Maestros.Cargo.Commands.RevocarCargo;

public class RevocarCargoValidator : AbstractValidator<RevocarCargoCommand>
{
    public RevocarCargoValidator()
    {
        RuleFor(x => x.Data.IdPersonaCargo)
            .GreaterThan(0).WithMessage("El ID de PersonaCargo debe ser mayor a 0");

        RuleFor(x => x.Data.FechaFin)
            .NotEmpty().WithMessage("La fecha de fin es requerida")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("La fecha de fin no puede ser futura");

        RuleFor(x => x.Data.MotivoRevocacion)
            .MaximumLength(255).WithMessage("El motivo de revocación no puede exceder 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Data.MotivoRevocacion));
    }
}
