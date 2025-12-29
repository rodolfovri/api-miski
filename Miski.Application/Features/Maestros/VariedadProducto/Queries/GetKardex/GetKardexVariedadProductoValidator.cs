using FluentValidation;

namespace Miski.Application.Features.Maestros.VariedadProducto.Queries.GetKardex;

public class GetKardexVariedadProductoValidator : AbstractValidator<GetKardexVariedadProductoQuery>
{
    public GetKardexVariedadProductoValidator()
    {
        RuleFor(x => x.IdVariedadProducto)
            .GreaterThan(0)
            .WithMessage("El ID de la variedad de producto debe ser mayor a 0");

        RuleFor(x => x.FechaDesde)
            .NotEmpty()
            .WithMessage("La fecha desde es requerida");

        RuleFor(x => x.FechaHasta)
            .NotEmpty()
            .WithMessage("La fecha hasta es requerida")
            .GreaterThanOrEqualTo(x => x.FechaDesde)
            .WithMessage("La fecha hasta debe ser mayor o igual a la fecha desde");

        RuleFor(x => x.TipoStock)
            .Must(ts => string.IsNullOrWhiteSpace(ts) || 
                        ts.Equals("MATERIA_PRIMA", StringComparison.OrdinalIgnoreCase) || 
                        ts.Equals("PRODUCTO_TERMINADO", StringComparison.OrdinalIgnoreCase))
            .WithMessage("El tipo de stock debe ser 'MATERIA_PRIMA' o 'PRODUCTO_TERMINADO'");
    }
}
