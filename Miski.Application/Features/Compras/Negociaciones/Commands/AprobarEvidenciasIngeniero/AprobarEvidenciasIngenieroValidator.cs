using FluentValidation;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.AprobarEvidenciasIngeniero;

public class AprobarEvidenciasIngenieroValidator : AbstractValidator<AprobarEvidenciasIngenieroCommand>
{
    public AprobarEvidenciasIngenieroValidator()
    {
        RuleFor(x => x.Aprobacion.IdNegociacion)
            .GreaterThan(0).WithMessage("El ID de la negociación debe ser mayor a 0");

        RuleFor(x => x.Aprobacion.AprobadaEvidenciasPorIngeniero)
            .GreaterThan(0).WithMessage("El ID del ingeniero aprobador debe ser mayor a 0");
    }
}
