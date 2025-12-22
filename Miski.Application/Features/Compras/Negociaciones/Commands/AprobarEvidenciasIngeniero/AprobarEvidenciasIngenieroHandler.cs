using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.AprobarEvidenciasIngeniero;

public class AprobarEvidenciasIngenieroHandler : IRequestHandler<AprobarEvidenciasIngenieroCommand, NegociacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AprobarEvidenciasIngenieroHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NegociacionDto> Handle(AprobarEvidenciasIngenieroCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Aprobacion;

        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(dto.IdNegociacion, cancellationToken);

        if (negociacion == null)
            throw new NotFoundException("Negociacion", dto.IdNegociacion);

        if (negociacion.Estado != "EN REVISION")
        {
            throw new ValidationException("Solo se pueden aprobar evidencias de negociaciones en estado 'EN REVISION'");
        }

        if (negociacion.EstadoAprobacionIngenieroEvidencias != "PENDIENTE")
        {
            throw new ValidationException("Las evidencias ya han sido procesadas por el ingeniero");
        }

        var aprobador = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(dto.AprobadaEvidenciasPorIngeniero, cancellationToken);
        
        if (aprobador == null)
            throw new NotFoundException("Usuario aprobador", dto.AprobadaEvidenciasPorIngeniero);

        negociacion.EstadoAprobacionIngenieroEvidencias = "APROBADO";
        negociacion.AprobadaEvidenciasPorIngeniero = dto.AprobadaEvidenciasPorIngeniero;
        negociacion.FAprobacionIngenieroEvidencias = DateTime.UtcNow;

        // Verificar si AMBOS (ingeniero y contadora) han aprobado las evidencias
        if (negociacion.EstadoAprobacionIngenieroEvidencias == "APROBADO" && 
            negociacion.EstadoAprobacionContadora == "APROBADO")
        {
            // Ambos aprobaron, crear la compra automáticamente
            negociacion.Estado = "FINALIZADO";

            var compra = new Compra
            {
                IdNegociacion = negociacion.IdNegociacion,
                IdMoneda = 1,
                IdTipoCambio = null,
                Serie = null,
                FRegistro = DateTime.UtcNow,
                FEmision = DateTime.UtcNow,
                Estado = "ACTIVO",
                EsParcial = "NO",
                MontoTotal = null,
                IGV = null,
                Observacion = null
            };

            await _unitOfWork.Repository<Compra>().AddAsync(compra, cancellationToken);
        }

        await _unitOfWork.Repository<Negociacion>().UpdateAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        negociacion.AprobadaEvidenciasPorUsuarioIngeniero = aprobador;
        negociacion.Comisionista = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(negociacion.IdComisionista, cancellationToken) ?? new Usuario();
        
        if (!string.IsNullOrEmpty(negociacion.NroDocumentoProveedor))
        {
            var proveedores = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
            negociacion.Proveedor = proveedores.FirstOrDefault(p => p.NumeroDocumento == negociacion.NroDocumentoProveedor);
        }

        if (negociacion.IdVariedadProducto.HasValue)
        {
            negociacion.VariedadProducto = await _unitOfWork.Repository<VariedadProducto>()
                .GetByIdAsync(negociacion.IdVariedadProducto.Value, cancellationToken);
            
            if (negociacion.VariedadProducto != null && negociacion.VariedadProducto.IdProducto > 0)
            {
                negociacion.VariedadProducto.Producto = await _unitOfWork.Repository<Producto>()
                    .GetByIdAsync(negociacion.VariedadProducto.IdProducto, cancellationToken);
            }
        }

        if (negociacion.AprobadaPorIngeniero.HasValue)
        {
            negociacion.AprobadaPorUsuarioIngeniero = await _unitOfWork.Repository<Usuario>()
                .GetByIdAsync(negociacion.AprobadaPorIngeniero.Value, cancellationToken);
        }

        if (negociacion.AprobadaPorContadora.HasValue)
        {
            negociacion.AprobadaPorUsuarioContadora = await _unitOfWork.Repository<Usuario>()
                .GetByIdAsync(negociacion.AprobadaPorContadora.Value, cancellationToken);
        }

        if (negociacion.IdTipoDocumento.HasValue)
        {
            negociacion.TipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
                .GetByIdAsync(negociacion.IdTipoDocumento.Value, cancellationToken);
        }

        if (negociacion.IdBanco.HasValue)
        {
            negociacion.Banco = await _unitOfWork.Repository<Banco>()
                .GetByIdAsync(negociacion.IdBanco.Value, cancellationToken);
        }

        return _mapper.Map<NegociacionDto>(negociacion);
    }
}
