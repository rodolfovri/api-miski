using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCambio.Commands.UpdateTipoCambio;

public class UpdateTipoCambioValidator : AbstractValidator<UpdateTipoCambioDto>
{
    public UpdateTipoCambioValidator()
    {
        RuleFor(x => x.IdTipoCambio)
            .GreaterThan(0)
            .WithMessage("El ID del tipo de cambio debe ser v�lido");

        RuleFor(x => x.IdMoneda)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar una moneda v�lida");

        RuleFor(x => x.IdUsuario)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un usuario v�lido");

        RuleFor(x => x.ValorCompra)
            .GreaterThan(0)
            .WithMessage("El valor de compra debe ser mayor a 0")
            .LessThan(1000)
            .WithMessage("El valor de compra no puede exceder 1000");

        RuleFor(x => x.ValorVenta)
            .GreaterThan(0)
            .WithMessage("El valor de venta debe ser mayor a 0")
            .LessThan(1000)
            .WithMessage("El valor de venta no puede exceder 1000");

        RuleFor(x => x)
            .Must(x => x.ValorVenta >= x.ValorCompra)
            .WithMessage("El valor de venta debe ser mayor o igual al valor de compra");
    }
}
