using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Moneda.Commands.UpdateMoneda;

public class UpdateMonedaValidator : AbstractValidator<UpdateMonedaDto>
{
    public UpdateMonedaValidator()
    {
        RuleFor(x => x.IdMoneda)
            .GreaterThan(0)
            .WithMessage("El ID de la moneda debe ser válido");

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Simbolo)
            .NotEmpty()
            .WithMessage("El símbolo es obligatorio")
            .MaximumLength(10)
            .WithMessage("El símbolo no puede exceder 10 caracteres");

        RuleFor(x => x.Codigo)
            .NotEmpty()
            .WithMessage("El código es obligatorio")
            .MaximumLength(10)
            .WithMessage("El código no puede exceder 10 caracteres")
            .Matches(@"^[A-Z]{3}$")
            .WithMessage("El código debe tener 3 letras mayúsculas (ej: USD, EUR, PEN)");
    }
}
