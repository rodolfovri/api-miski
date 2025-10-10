using FluentValidation;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Commands.AsignarPermiso;

public class AsignarPermisoValidator : AbstractValidator<AsignarPermisoDto>
{
    public AsignarPermisoValidator()
    {
        RuleFor(x => x.IdRol)
            .GreaterThan(0)
            .WithMessage("El ID del rol es requerido");

        RuleFor(x => x)
            .Must(HaveAtLeastOneLevel)
            .WithMessage("Debe especificar al menos un nivel de permiso (Módulo, SubMódulo o Detalle)");
    }

    private bool HaveAtLeastOneLevel(AsignarPermisoDto dto)
    {
        return dto.IdModulo.HasValue || dto.IdSubModulo.HasValue || dto.IdSubModuloDetalle.HasValue;
    }
}