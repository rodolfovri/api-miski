using FluentValidation;
using Miski.Shared.DTOs;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;

public class CreateNegociacionValidator : AbstractValidator<CreateNegociacionCommand>
{
    public CreateNegociacionValidator()
    {
        RuleFor(x => x.Negociacion.IdComisionista)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un comisionista v�lido");

        RuleFor(x => x.Negociacion.Fecha)
            .NotEmpty()
            .WithMessage("La fecha es requerida")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1))
            .WithMessage("La fecha no puede ser futura")
            .GreaterThanOrEqualTo(DateTime.Now.AddDays(-30))
            .WithMessage("La fecha no puede ser mayor a 30 d�as en el pasado");

        RuleFor(x => x.Negociacion.PesoTotal)
            .GreaterThan(0)
            .WithMessage("El peso total debe ser mayor que 0")
            .LessThanOrEqualTo(50000) // 50 toneladas m�ximo
            .WithMessage("El peso total no puede exceder 50,000 kg");

        RuleFor(x => x.Negociacion.SacosTotales)
            .GreaterThan(0)
            .WithMessage("El n�mero de sacos debe ser mayor que 0")
            .LessThanOrEqualTo(2000) // m�ximo 2000 sacos
            .WithMessage("El n�mero de sacos no puede exceder 2,000 unidades");

        RuleFor(x => x.Negociacion.PrecioUnitario)
            .GreaterThan(0)
            .WithMessage("El precio unitario debe ser mayor que 0")
            .LessThanOrEqualTo(1000) // m�ximo 1000 soles por kg
            .WithMessage("El precio unitario no puede exceder S/. 1,000 por kg");

        RuleFor(x => x.Negociacion.NroCuentaRuc)
            .NotEmpty()
            .WithMessage("El n�mero de cuenta/RUC es requerido")
            .MaximumLength(20)
            .WithMessage("El n�mero de cuenta/RUC es demasiado largo")
            .MinimumLength(8)
            .WithMessage("El n�mero de cuenta/RUC es demasiado corto");

        RuleFor(x => x.Negociacion.FotoCalidadProducto)
            .NotEmpty()
            .WithMessage("La foto de calidad del producto es requerida");

        RuleFor(x => x.Negociacion.FotoDniFrontal)
            .NotEmpty()
            .WithMessage("La foto frontal del DNI es requerida");

        RuleFor(x => x.Negociacion.FotoDniPosterior)
            .NotEmpty()
            .WithMessage("La foto posterior del DNI es requerida");

        RuleFor(x => x.Negociacion.Observacion)
            .MaximumLength(500)
            .WithMessage("Las observaciones son demasiado largas");

        // Validaci�n condicional para proveedor
        When(x => x.Negociacion.IdProveedor.HasValue, () =>
        {
            RuleFor(x => x.Negociacion.IdProveedor.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un proveedor v�lido");
        });

        // Validaci�n condicional para producto
        When(x => x.Negociacion.IdProducto.HasValue, () =>
        {
            RuleFor(x => x.Negociacion.IdProducto.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un producto v�lido");
        });

        // Validaci�n de l�gica de negocio: coherencia entre peso y sacos
        RuleFor(x => x.Negociacion)
            .Must(HaveConsistentWeightAndBags)
            .WithMessage("La relaci�n entre peso y n�mero de sacos no es coherente");
    }

    private static bool HaveConsistentWeightAndBags(CreateNegociacionDto negociacion)
    {
        if (negociacion.SacosTotales == 0) return false;
        
        var avgWeightPerBag = negociacion.PesoTotal / negociacion.SacosTotales;
        
        // Un saco t�pico de productos agr�colas pesa entre 20-30 kg
        return avgWeightPerBag >= 20 && avgWeightPerBag <= 30;
    }
}