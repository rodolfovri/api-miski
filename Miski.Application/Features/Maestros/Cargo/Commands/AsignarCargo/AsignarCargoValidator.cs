using FluentValidation;

namespace Miski.Application.Features.Maestros.Cargo.Commands.AsignarCargo;

public class AsignarCargoValidator : AbstractValidator<AsignarCargoCommand>
{
    public AsignarCargoValidator()
    {
        RuleFor(x => x.Data.IdPersona)
            .GreaterThan(0).WithMessage("El ID de la persona debe ser mayor a 0");

        RuleFor(x => x.Data.IdCargo)
            .GreaterThan(0).WithMessage("El ID del cargo debe ser mayor a 0");

        RuleFor(x => x.Data.FechaInicio)
            .NotEmpty().WithMessage("La fecha de inicio es requerida")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("La fecha de inicio no puede ser futura");

        RuleFor(x => x.Data.ObservacionAsignacion)
            .MaximumLength(255).WithMessage("La observación de asignación no puede exceder 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Data.ObservacionAsignacion));
    }
}
