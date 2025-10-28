using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Vehiculo.Commands.CreateVehiculo;

public class CreateVehiculoValidator : AbstractValidator<CreateVehiculoCommand>
{
    public CreateVehiculoValidator()
    {
        RuleFor(x => x.Vehiculo.Placa)
            .NotEmpty().WithMessage("La placa es requerida")
            .MaximumLength(20).WithMessage("La placa no puede exceder 20 caracteres")
            .Matches(@"^[A-Z0-9-]+$").WithMessage("La placa solo puede contener letras mayúsculas, números y guiones");

        RuleFor(x => x.Vehiculo.Marca)
            .MaximumLength(50).WithMessage("La marca no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Vehiculo.Marca));

        RuleFor(x => x.Vehiculo.Modelo)
            .MaximumLength(50).WithMessage("El modelo no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Vehiculo.Modelo));
    }
}
