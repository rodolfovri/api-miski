using FluentValidation;

namespace Miski.Application.Features.Compras.Compras.Commands.ToggleCompraParcial;

public class ToggleCompraParcialValidator : AbstractValidator<ToggleCompraParcialCommand>
{
    public ToggleCompraParcialValidator()
    {
        RuleFor(x => x.IdCompra)
            .GreaterThan(0).WithMessage("El ID de la compra debe ser mayor a 0");
    }
}
