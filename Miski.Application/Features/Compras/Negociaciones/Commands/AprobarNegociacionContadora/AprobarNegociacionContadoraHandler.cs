using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.AprobarNegociacionContadora;

public class AprobarNegociacionContadoraHandler : IRequestHandler<AprobarNegociacionContadoraCommand, NegociacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AprobarNegociacionContadoraHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NegociacionDto> Handle(AprobarNegociacionContadoraCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Aprobacion;

        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(dto.IdNegociacion, cancellationToken);

        if (negociacion == null)
            throw new NotFoundException("Negociacion", dto.IdNegociacion);

        // Validar que la negociaci�n est� en revisi�n
        if (negociacion.Estado != "EN REVISI�N")
        {
            throw new ValidationException("Solo se pueden aprobar negociaciones en estado 'EN REVISI�N'");
        }

        // Validar que est� pendiente de aprobaci�n por contadora
        if (negociacion.EstadoAprobacionContadora != "PENDIENTE")
        {
            throw new ValidationException("La negociaci�n ya ha sido procesada por la contadora");
        }

        // Validar que el usuario aprobador existe
        var aprobador = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(dto.AprobadaPorContadora, cancellationToken);
        
        if (aprobador == null)
            throw new NotFoundException("Usuario aprobador", dto.AprobadaPorContadora);

        // Aprobar la negociaci�n
        negociacion.EstadoAprobacionContadora = "APROBADO";
        negociacion.AprobadaPorContadora = dto.AprobadaPorContadora;
        negociacion.FAprobacionContadora = DateTime.Now;
        negociacion.Estado = "FINALIZADO"; // Cambia el estado general a FINALIZADO

        await _unitOfWork.Repository<Negociacion>().UpdateAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ? CREAR LA COMPRA AUTOM�TICAMENTE
        var compra = new Compra
        {
            IdNegociacion = negociacion.IdNegociacion,
            IdMoneda = 1, // Moneda por defecto (PEN - Soles)
            IdTipoCambio = null, // Se puede asignar despu�s si es necesario
            Serie = null, // Se puede generar despu�s
            FRegistro = DateTime.Now,
            FEmision = DateTime.Now,
            Estado = "ACTIVO",
            // MontoTotal, IGV y Observacion se dejan null inicialmente
            MontoTotal = null,
            IGV = null,
            Observacion = null
        };

        await _unitOfWork.Repository<Compra>().AddAsync(compra, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        negociacion.AprobadaPorUsuarioContadora = aprobador;
        negociacion.Comisionista = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(negociacion.IdComisionista, cancellationToken);
        
        if (!string.IsNullOrEmpty(negociacion.NroDocumentoProveedor))
        {
            var proveedores = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
            negociacion.Proveedor = proveedores.FirstOrDefault(p => p.NumeroDocumento == negociacion.NroDocumentoProveedor);
        }

        if (negociacion.IdVariedadProducto.HasValue)
        {
            negociacion.VariedadProducto = await _unitOfWork.Repository<VariedadProducto>()
                .GetByIdAsync(negociacion.IdVariedadProducto.Value, cancellationToken);
        }

        if (negociacion.AprobadaPorIngeniero.HasValue)
        {
            negociacion.AprobadaPorUsuarioIngeniero = await _unitOfWork.Repository<Usuario>()
                .GetByIdAsync(negociacion.AprobadaPorIngeniero.Value, cancellationToken);
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
