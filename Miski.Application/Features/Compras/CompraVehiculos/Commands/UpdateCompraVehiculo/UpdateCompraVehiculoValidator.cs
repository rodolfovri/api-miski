using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Commands.UpdateCompraVehiculo;

public class UpdateCompraVehiculoValidator : AbstractValidator<UpdateCompraVehiculoDto>
{
    public UpdateCompraVehiculoValidator()
    {
        RuleFor(x => x.IdCompraVehiculo)
            .GreaterThan(0)
            .WithMessage("El ID de la asignaci�n debe ser v�lido");

        RuleFor(x => x.IdPersona)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar una persona v�lida");

        RuleFor(x => x.IdVehiculo)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un veh�culo v�lido");

        RuleFor(x => x.GuiaRemision)
            .NotEmpty()
            .WithMessage("La gu�a de remisi�n es obligatoria")
            .MaximumLength(50)
            .WithMessage("La gu�a de remisi�n no puede exceder 50 caracteres");

        RuleFor(x => x.IdCompras)
            .NotEmpty()
            .WithMessage("Debe seleccionar al menos una compra")
            .Must(compras => compras.Count > 0)
            .WithMessage("Debe asignar al menos una compra al veh�culo");

        RuleForEach(x => x.IdCompras)
            .GreaterThan(0)
            .WithMessage("Todos los IDs de compras deben ser v�lidos");
    }
}
