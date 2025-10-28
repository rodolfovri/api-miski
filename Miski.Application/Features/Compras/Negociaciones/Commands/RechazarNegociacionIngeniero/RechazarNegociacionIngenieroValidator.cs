using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.RechazarNegociacionIngeniero;

public class RechazarNegociacionIngenieroValidator : AbstractValidator<RechazarNegociacionIngenieroCommand>
{
    public RechazarNegociacionIngenieroValidator()
    {
        RuleFor(x => x.Rechazo.IdNegociacion)
            .GreaterThan(0)
            .WithMessage("El ID de la negociación es requerido");

        RuleFor(x => x.Rechazo.RechazadoPorIngeniero)
            .GreaterThan(0)
            .WithMessage("Debe especificar el usuario que rechaza");
    }
}
