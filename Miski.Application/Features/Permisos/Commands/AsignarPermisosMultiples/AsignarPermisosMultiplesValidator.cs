using FluentValidation;

namespace Miski.Application.Features.Permisos.Commands.AsignarPermisosMultiples;

public class AsignarPermisosMultiplesValidator : AbstractValidator<AsignarPermisosMultiplesCommand>
{
    public AsignarPermisosMultiplesValidator()
    {
        RuleFor(x => x.Data.IdRol)
            .GreaterThan(0)
            .WithMessage("El ID del rol debe ser mayor que 0");

        RuleFor(x => x.Data.Permisos)
            .NotEmpty()
            .WithMessage("Debe especificar al menos un permiso")
            .Must(permisos => permisos != null && permisos.Count > 0)
            .WithMessage("La lista de permisos no puede estar vacía");

        RuleForEach(x => x.Data.Permisos).ChildRules(permiso =>
        {
            permiso.RuleFor(p => p.IdModulo)
                .GreaterThan(0)
                .When(p => p.IdModulo.HasValue)
                .WithMessage("El ID del módulo debe ser mayor que 0");

            permiso.RuleFor(p => p.IdSubModulo)
                .GreaterThan(0)
                .When(p => p.IdSubModulo.HasValue)
                .WithMessage("El ID del submódulo debe ser mayor que 0");

            permiso.RuleFor(p => p.IdSubModuloDetalle)
                .GreaterThan(0)
                .When(p => p.IdSubModuloDetalle.HasValue)
                .WithMessage("El ID del detalle del submódulo debe ser mayor que 0");

            // Validar que al menos se especifique un nivel
            permiso.RuleFor(p => p)
                .Must(p => p.IdModulo.HasValue || p.IdSubModulo.HasValue || p.IdSubModuloDetalle.HasValue)
                .WithMessage("Debe especificar al menos un ID (Módulo, SubMódulo o SubMóduloDetalle)");
        });
    }
}
