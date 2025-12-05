using FluentValidation;

namespace Miski.Application.Features.Compras.CompraPagos.Commands.RegistrarAbono;

public class RegistrarAbonoValidator : AbstractValidator<RegistrarAbonoCommand>
{
    public RegistrarAbonoValidator()
    {
        RuleFor(x => x.IdCompra)
            .GreaterThan(0)
            .WithMessage("El ID de la compra es requerido");

        RuleFor(x => x.MontoAbono)
            .GreaterThan(0)
            .WithMessage("El monto del abono debe ser mayor a 0");
    }
}
