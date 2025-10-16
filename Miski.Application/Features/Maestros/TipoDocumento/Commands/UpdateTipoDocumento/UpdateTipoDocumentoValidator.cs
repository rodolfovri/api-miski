using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoDocumento.Commands.UpdateTipoDocumento;

public class UpdateTipoDocumentoValidator : AbstractValidator<UpdateTipoDocumentoDto>
{
    public UpdateTipoDocumentoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(50)
            .WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.LongitudMin)
            .GreaterThan(0)
            .When(x => x.LongitudMin.HasValue)
            .WithMessage("La longitud mínima debe ser mayor a 0");

        RuleFor(x => x.LongitudMax)
            .GreaterThan(0)
            .When(x => x.LongitudMax.HasValue)
            .WithMessage("La longitud máxima debe ser mayor a 0")
            .GreaterThanOrEqualTo(x => x.LongitudMin ?? 0)
            .When(x => x.LongitudMin.HasValue && x.LongitudMax.HasValue)
            .WithMessage("La longitud máxima debe ser mayor o igual a la longitud mínima");
    }
}