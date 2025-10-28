using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CompletarNegociacion;

public class CompletarNegociacionValidator : AbstractValidator<CompletarNegociacionCommand>
{
    public CompletarNegociacionValidator()
    {
        RuleFor(x => x.Completar.IdNegociacion)
            .GreaterThan(0)
            .WithMessage("El ID de la negociaci�n es requerido");

        // Validaci�n condicional para IdTipoDocumento si se proporciona
        When(x => x.Completar.IdTipoDocumento.HasValue, () =>
        {
            RuleFor(x => x.Completar.IdTipoDocumento!.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un tipo de documento v�lido");
        });

        // Validaci�n condicional para IdBanco si se proporciona
        When(x => x.Completar.IdBanco.HasValue, () =>
        {
            RuleFor(x => x.Completar.IdBanco!.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un banco v�lido");
        });

        RuleFor(x => x.Completar.NroDocumentoProveedor)
            .NotEmpty()
            .WithMessage("El n�mero de documento del proveedor es requerido")
            .MinimumLength(8)
            .WithMessage("El n�mero de documento debe tener al menos 8 caracteres")
            .MaximumLength(20)
            .WithMessage("El n�mero de documento no puede exceder 20 caracteres");

        RuleFor(x => x.Completar.NroCuentaBancaria)
            .NotEmpty()
            .WithMessage("El n�mero de cuenta bancaria es requerido")
            .MinimumLength(10)
            .WithMessage("El n�mero de cuenta debe tener al menos 10 caracteres")
            .MaximumLength(30)
            .WithMessage("El n�mero de cuenta no puede exceder 30 caracteres");

        RuleFor(x => x.Completar.FotoDniFrontal)
            .NotNull()
            .WithMessage("La foto del DNI frontal es requerida");

        RuleFor(x => x.Completar.FotoDniPosterior)
            .NotNull()
            .WithMessage("La foto del DNI posterior es requerida");

        RuleFor(x => x.Completar.PrimeraEvidenciaFoto)
            .NotNull()
            .WithMessage("La primera evidencia fotogr�fica es requerida");

        RuleFor(x => x.Completar.SegundaEvidenciaFoto)
            .NotNull()
            .WithMessage("La segunda evidencia fotogr�fica es requerida");

        RuleFor(x => x.Completar.TerceraEvidenciaFoto)
            .NotNull()
            .WithMessage("La tercera evidencia fotogr�fica es requerida");

        RuleFor(x => x.Completar.EvidenciaVideo)
            .NotNull()
            .WithMessage("La evidencia en video es requerida");
    }
}
