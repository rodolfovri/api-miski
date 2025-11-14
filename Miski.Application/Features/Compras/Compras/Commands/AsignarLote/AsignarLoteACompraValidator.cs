using FluentValidation;

namespace Miski.Application.Features.Compras.Compras.Commands.AsignarLote;

public class AsignarLoteACompraValidator : AbstractValidator<AsignarLoteACompraCommand>
{
    public AsignarLoteACompraValidator()
    {
        RuleFor(x => x.IdCompra)
            .GreaterThan(0)
            .WithMessage("El ID de la compra debe ser mayor a 0");

        RuleFor(x => x.IdLote)
            .GreaterThan(0)
            .WithMessage("El ID del lote debe ser mayor a 0");

        RuleFor(x => x.MontoTotal)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto total no puede ser negativo");
    }
}
