using FluentValidation;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Commands.UpdateUbicacion;

public class UpdateUbicacionValidator : AbstractValidator<UpdateUbicacionDto>
{
    public UpdateUbicacionValidator()
    {
        RuleFor(x => x.IdUbicacion)
            .GreaterThan(0)
            .WithMessage("El ID de la ubicación es requerido");

        RuleFor(x => x.IdUsuario)
            .GreaterThan(0)
            .WithMessage("El ID del usuario es requerido");

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Direccion)
            .NotEmpty()
            .WithMessage("La dirección es requerida")
            .MaximumLength(200)
            .WithMessage("La dirección no puede exceder 200 caracteres");

        RuleFor(x => x.Tipo)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Tipo))
            .WithMessage("El tipo no puede exceder 50 caracteres");
    }
}