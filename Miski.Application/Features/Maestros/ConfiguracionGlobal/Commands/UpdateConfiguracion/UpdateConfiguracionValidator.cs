using FluentValidation;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.UpdateConfiguracion;

public class UpdateConfiguracionValidator : AbstractValidator<UpdateConfiguracionCommand>
{
    public UpdateConfiguracionValidator()
    {
        RuleFor(x => x.ConfiguracionData.IdConfiguracionGlobal)
            .GreaterThan(0).WithMessage("El ID de configuración debe ser mayor a 0");

        RuleFor(x => x.ConfiguracionData.Valor)
            .NotEmpty().WithMessage("El valor es requerido")
            .MaximumLength(255).WithMessage("El valor no puede exceder 255 caracteres");

        RuleFor(x => x.ConfiguracionData.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");
    }
}
