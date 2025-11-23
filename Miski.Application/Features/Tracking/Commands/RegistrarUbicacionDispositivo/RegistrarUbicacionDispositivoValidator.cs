using FluentValidation;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Commands.RegistrarUbicacionDispositivo;

public class RegistrarUbicacionDispositivoValidator : AbstractValidator<UbicacionDispositivoDto>
{
    public RegistrarUbicacionDispositivoValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("DeviceId es requerido")
            .MaximumLength(255).WithMessage("DeviceId no puede exceder 255 caracteres");

        RuleFor(x => x.IdPersona)
            .GreaterThan(0).WithMessage("IdPersona debe ser mayor a 0");

        RuleFor(x => x.Latitud)
            .NotEmpty().WithMessage("Latitud es requerida")
            .Must(BeValidLatitude).WithMessage("Latitud debe estar entre -90 y 90");

        RuleFor(x => x.Longitud)
            .NotEmpty().WithMessage("Longitud es requerida")
            .Must(BeValidLongitude).WithMessage("Longitud debe estar entre -180 y 180");

        RuleFor(x => x.Precision)
            .GreaterThanOrEqualTo(0).When(x => x.Precision.HasValue)
            .WithMessage("Precisión debe ser un valor positivo");

        RuleFor(x => x.Velocidad)
            .GreaterThanOrEqualTo(0).When(x => x.Velocidad.HasValue)
            .WithMessage("Velocidad debe ser un valor positivo");
    }

    private bool BeValidLatitude(string latitud)
    {
        if (string.IsNullOrWhiteSpace(latitud)) return false;
        if (!decimal.TryParse(latitud, out var lat)) return false;
        return lat >= -90 && lat <= 90;
    }

    private bool BeValidLongitude(string longitud)
    {
        if (string.IsNullOrWhiteSpace(longitud)) return false;
        if (!decimal.TryParse(longitud, out var lng)) return false;
        return lng >= -180 && lng <= 180;
    }
}
