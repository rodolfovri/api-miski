using FluentValidation;

namespace Miski.Application.Features.Compras.CompraPagos.Commands.DefinirTipoPago;

public class DefinirTipoPagoValidator : AbstractValidator<DefinirTipoPagoCommand>
{
    public DefinirTipoPagoValidator()
    {
        RuleFor(x => x.IdCompra)
            .GreaterThan(0)
            .WithMessage("El ID de la compra es requerido");

        RuleFor(x => x.TipoPago)
            .NotEmpty()
            .WithMessage("El tipo de pago es requerido")
            .Must(x => x == "CONTADO" || x == "CREDITO")
            .WithMessage("El tipo de pago debe ser CONTADO o CREDITO");

        RuleFor(x => x.DiasCredito)
            .GreaterThan(0)
            .When(x => x.TipoPago == "CREDITO" && x.DiasCredito.HasValue)
            .WithMessage("Los días de crédito deben ser mayor a 0");

        RuleFor(x => x.MontoAdelanto)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MontoAdelanto.HasValue)
            .WithMessage("El monto de adelanto no puede ser negativo");
    }
}
