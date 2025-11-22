using FluentValidation;

namespace Miski.Application.Features.Ubicaciones.Commands.AsignarUbicacion;

public class AsignarUbicacionValidator : AbstractValidator<AsignarUbicacionCommand>
{
    public AsignarUbicacionValidator()
    {
        RuleFor(x => x.Data.IdPersona)
            .GreaterThan(0)
            .WithMessage("El ID de la persona debe ser mayor a 0");

        RuleFor(x => x.Data.IdUbicacion)
            .GreaterThan(0)
            .WithMessage("El ID de la ubicación debe ser mayor a 0");
    }
}
