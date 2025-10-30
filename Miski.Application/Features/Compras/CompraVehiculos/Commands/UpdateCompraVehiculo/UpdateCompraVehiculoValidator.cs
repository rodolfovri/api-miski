using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Commands.UpdateCompraVehiculo;

public class UpdateCompraVehiculoValidator : AbstractValidator<UpdateCompraVehiculoDto>
{
    public UpdateCompraVehiculoValidator()
    {
        RuleFor(x => x.IdCompraVehiculo)
            .GreaterThan(0)
            .WithMessage("El ID de la asignación debe ser válido");

        RuleFor(x => x.IdPersona)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar una persona válida");

        RuleFor(x => x.IdVehiculo)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un vehículo válido");

        RuleFor(x => x.GuiaRemision)
            .NotEmpty()
            .WithMessage("La guía de remisión es obligatoria")
            .MaximumLength(50)
            .WithMessage("La guía de remisión no puede exceder 50 caracteres");

        RuleFor(x => x.IdCompras)
            .NotEmpty()
            .WithMessage("Debe seleccionar al menos una compra")
            .Must(compras => compras.Count > 0)
            .WithMessage("Debe asignar al menos una compra al vehículo");

        RuleForEach(x => x.IdCompras)
            .GreaterThan(0)
            .WithMessage("Todos los IDs de compras deben ser válidos");
    }
}
