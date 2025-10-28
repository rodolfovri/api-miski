using FluentValidation;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Commands.CreateUbicacion;

public class CreateUbicacionValidator : AbstractValidator<CreateUbicacionDto>
{
    public CreateUbicacionValidator()
    {
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0)
            .WithMessage("El ID del usuario es requerido");

        RuleFor(x => x.CodigoSenasa)
            .MaximumLength(50)
            .WithMessage("El código SENASA no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.CodigoSenasa));

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.RazonSocial)
            .NotEmpty()
            .WithMessage("La razón social es requerida")
            .MaximumLength(150)
            .WithMessage("La razón social no puede exceder 150 caracteres");

        RuleFor(x => x.NumeroRuc)
            .MaximumLength(20)
            .WithMessage("El número de RUC no puede exceder 20 caracteres")
            .Matches(@"^\d{11}$")
            .WithMessage("El RUC debe tener 11 dígitos")
            .When(x => !string.IsNullOrEmpty(x.NumeroRuc));

        RuleFor(x => x.Direccion)
            .NotEmpty()
            .WithMessage("La dirección es requerida")
            .MaximumLength(100)
            .WithMessage("La dirección no puede exceder 100 caracteres");

        RuleFor(x => x.DomicilioLegal)
            .MaximumLength(100)
            .WithMessage("El domicilio legal no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.DomicilioLegal));

        RuleFor(x => x.GiroEstablecimiento)
            .MaximumLength(100)
            .WithMessage("El giro del establecimiento no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.GiroEstablecimiento));

        RuleFor(x => x.ComprobantePdf)
            .Must(file => file == null || file.ContentType == "application/pdf")
            .WithMessage("El comprobante debe ser un archivo PDF")
            .When(x => x.ComprobantePdf != null);

        RuleFor(x => x.ComprobantePdf)
            .Must(file => file == null || file.Length <= 5 * 1024 * 1024) // 5 MB
            .WithMessage("El PDF no puede exceder 5 MB")
            .When(x => x.ComprobantePdf != null);

        RuleFor(x => x.Tipo)
            .MaximumLength(50)
            .WithMessage("El tipo no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Tipo));

        RuleFor(x => x.Estado)
            .Must(estado => estado == "ACTIVO" || estado == "INACTIVO")
            .WithMessage("El estado debe ser ACTIVO o INACTIVO");
    }
}