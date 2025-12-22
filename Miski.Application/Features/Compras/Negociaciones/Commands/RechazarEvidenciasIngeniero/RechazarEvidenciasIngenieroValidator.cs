using FluentValidation;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.RechazarEvidenciasIngeniero;

public class RechazarEvidenciasIngenieroValidator : AbstractValidator<RechazarEvidenciasIngenieroCommand>
{
    public RechazarEvidenciasIngenieroValidator()
    {
        RuleFor(x => x.Rechazo.IdNegociacion)
            .GreaterThan(0).WithMessage("El ID de la negociación debe ser mayor a 0");

        RuleFor(x => x.Rechazo.RechazadaEvidenciasPorIngeniero)
            .GreaterThan(0).WithMessage("El ID del ingeniero rechazador debe ser mayor a 0");
    }
}
