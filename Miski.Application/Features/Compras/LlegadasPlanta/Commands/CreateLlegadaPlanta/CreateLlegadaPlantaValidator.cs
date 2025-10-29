using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Commands.CreateLlegadaPlanta;

public class CreateLlegadaPlantaValidator : AbstractValidator<CreateLlegadaPlantaDto>
{
    public CreateLlegadaPlantaValidator()
    {
        RuleFor(x => x.IdCompraVehiculo)
            .GreaterThan(0)
            .WithMessage("El ID del CompraVehiculo es obligatorio");

        RuleFor(x => x.IdUsuario)
            .GreaterThan(0)
            .WithMessage("El ID del usuario es obligatorio");

        RuleFor(x => x.Detalles)
            .NotEmpty()
            .WithMessage("Debe incluir al menos un detalle de lote recibido");

        RuleForEach(x => x.Detalles).ChildRules(detalle =>
        {
            detalle.RuleFor(d => d.IdCompra)
                .GreaterThan(0)
                .WithMessage("El ID de la compra es obligatorio");

            detalle.RuleFor(d => d.IdLote)
                .GreaterThan(0)
                .WithMessage("El ID del lote es obligatorio");

            detalle.RuleFor(d => d.SacosRecibidos)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Los sacos recibidos deben ser mayor o igual a 0");

            detalle.RuleFor(d => d.PesoRecibido)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El peso recibido debe ser mayor o igual a 0");
        });
    }
}
