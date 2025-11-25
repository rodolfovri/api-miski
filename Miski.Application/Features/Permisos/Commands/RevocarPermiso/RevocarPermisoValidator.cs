using FluentValidation;

namespace Miski.Application.Features.Permisos.Commands.RevocarPermiso;

public class RevocarPermisoValidator : AbstractValidator<RevocarPermisoCommand>
{
    public RevocarPermisoValidator()
    {
        RuleFor(x => x.IdRol)
            .GreaterThan(0)
            .WithMessage("El ID del rol debe ser mayor que 0");

        RuleFor(x => x.IdModulo)
            .GreaterThan(0)
            .When(x => x.IdModulo.HasValue)
            .WithMessage("El ID del módulo debe ser mayor que 0");

        RuleFor(x => x.IdSubModulo)
            .GreaterThan(0)
            .When(x => x.IdSubModulo.HasValue)
            .WithMessage("El ID del submódulo debe ser mayor que 0");

        RuleFor(x => x.IdSubModuloDetalle)
            .GreaterThan(0)
            .When(x => x.IdSubModuloDetalle.HasValue)
            .WithMessage("El ID del detalle del submódulo debe ser mayor que 0");

        // Validar que al menos se especifique un nivel
        RuleFor(x => x)
            .Must(x => x.IdModulo.HasValue || x.IdSubModulo.HasValue || x.IdSubModuloDetalle.HasValue)
            .WithMessage("Debe especificar al menos un ID (Módulo, SubMódulo o SubMóduloDetalle) para revocar");
    }
}
