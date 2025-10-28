using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;

public class CreateNegociacionValidator : AbstractValidator<CreateNegociacionCommand>
{
    public CreateNegociacionValidator()
    {
        RuleFor(x => x.Negociacion.IdComisionista)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un comisionista válido");

        RuleFor(x => x.Negociacion.TipoCalidad)
            .NotEmpty()
            .WithMessage("El tipo de calidad es requerido")
            .MaximumLength(50)
            .WithMessage("El tipo de calidad no puede exceder 50 caracteres");

        // Validación condicional para IdVariedadProducto si se proporciona
        When(x => x.Negociacion.IdVariedadProducto.HasValue, () =>
        {
            RuleFor(x => x.Negociacion.IdVariedadProducto!.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar una variedad de producto válida");
        });

        When(x => x.Negociacion.SacosTotales.HasValue, () =>
        {
            RuleFor(x => x.Negociacion.SacosTotales!.Value)
                .GreaterThan(0)
                .WithMessage("El número de sacos debe ser mayor que 0")
                .LessThanOrEqualTo(2000)
                .WithMessage("El número de sacos no puede exceder 2,000 unidades");
        });

        When(x => x.Negociacion.PrecioUnitario.HasValue, () =>
        {
            RuleFor(x => x.Negociacion.PrecioUnitario!.Value)
                .GreaterThan(0)
                .WithMessage("El precio unitario debe ser mayor que 0")
                .LessThanOrEqualTo(1000)
                .WithMessage("El precio unitario no puede exceder S/. 1,000 por kg");
        });
    }
}