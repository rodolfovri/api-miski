using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Moneda.Commands.UpdateMoneda;

public class UpdateMonedaValidator : AbstractValidator<UpdateMonedaDto>
{
    public UpdateMonedaValidator()
    {
        RuleFor(x => x.IdMoneda)
            .GreaterThan(0)
            .WithMessage("El ID de la moneda debe ser v�lido");

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Simbolo)
            .NotEmpty()
            .WithMessage("El s�mbolo es obligatorio")
            .MaximumLength(10)
            .WithMessage("El s�mbolo no puede exceder 10 caracteres");

        RuleFor(x => x.Codigo)
            .NotEmpty()
            .WithMessage("El c�digo es obligatorio")
            .MaximumLength(10)
            .WithMessage("El c�digo no puede exceder 10 caracteres")
            .Matches(@"^[A-Z]{3}$")
            .WithMessage("El c�digo debe tener 3 letras may�sculas (ej: USD, EUR, PEN)");
    }
}
