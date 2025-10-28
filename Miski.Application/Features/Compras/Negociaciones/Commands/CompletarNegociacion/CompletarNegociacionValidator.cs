using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CompletarNegociacion;

public class CompletarNegociacionValidator : AbstractValidator<CompletarNegociacionCommand>
{
    public CompletarNegociacionValidator()
    {
        RuleFor(x => x.Completar.IdNegociacion)
            .GreaterThan(0)
            .WithMessage("El ID de la negociación es requerido");

        // Validación condicional para IdTipoDocumento si se proporciona
        When(x => x.Completar.IdTipoDocumento.HasValue, () =>
        {
            RuleFor(x => x.Completar.IdTipoDocumento!.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un tipo de documento válido");
        });

        // Validación condicional para IdBanco si se proporciona
        When(x => x.Completar.IdBanco.HasValue, () =>
        {
            RuleFor(x => x.Completar.IdBanco!.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un banco válido");
        });

        RuleFor(x => x.Completar.NroDocumentoProveedor)
            .NotEmpty()
            .WithMessage("El número de documento del proveedor es requerido")
            .MinimumLength(8)
            .WithMessage("El número de documento debe tener al menos 8 caracteres")
            .MaximumLength(20)
            .WithMessage("El número de documento no puede exceder 20 caracteres");

        RuleFor(x => x.Completar.NroCuentaBancaria)
            .NotEmpty()
            .WithMessage("El número de cuenta bancaria es requerido")
            .MinimumLength(10)
            .WithMessage("El número de cuenta debe tener al menos 10 caracteres")
            .MaximumLength(30)
            .WithMessage("El número de cuenta no puede exceder 30 caracteres");

        RuleFor(x => x.Completar.FotoDniFrontal)
            .NotNull()
            .WithMessage("La foto del DNI frontal es requerida");

        RuleFor(x => x.Completar.FotoDniPosterior)
            .NotNull()
            .WithMessage("La foto del DNI posterior es requerida");

        RuleFor(x => x.Completar.PrimeraEvidenciaFoto)
            .NotNull()
            .WithMessage("La primera evidencia fotográfica es requerida");

        RuleFor(x => x.Completar.SegundaEvidenciaFoto)
            .NotNull()
            .WithMessage("La segunda evidencia fotográfica es requerida");

        RuleFor(x => x.Completar.TerceraEvidenciaFoto)
            .NotNull()
            .WithMessage("La tercera evidencia fotográfica es requerida");

        RuleFor(x => x.Completar.EvidenciaVideo)
            .NotNull()
            .WithMessage("La evidencia en video es requerida");
    }
}
