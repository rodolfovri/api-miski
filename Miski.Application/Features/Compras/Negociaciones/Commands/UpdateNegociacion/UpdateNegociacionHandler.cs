using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.UpdateNegociacion;

public class UpdateNegociacionHandler : IRequestHandler<UpdateNegociacionCommand, NegociacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const decimal PESO_POR_SACO_DEFAULT = 50m; // Peso por defecto en kg

    public UpdateNegociacionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NegociacionDto> Handle(UpdateNegociacionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Negociacion;

        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (negociacion == null)
            throw new NotFoundException("Negociacion", request.Id);

        // Solo se puede actualizar si está en estado 'EN PROCESO' y EstadoAprobacionIngeniero 'PENDIENTE'
        if (negociacion.Estado != "EN PROCESO" || negociacion.EstadoAprobacionIngeniero != "PENDIENTE")
        {
            throw new ValidationException("Solo se pueden editar negociaciones en estado 'EN PROCESO' con EstadoAprobacionIngeniero 'PENDIENTE'");
        }

        // Validar que el comisionista existe
        var comisionista = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(dto.IdComisionista, cancellationToken);
        
        if (comisionista == null)
            throw new NotFoundException("Comisionista", dto.IdComisionista);

        // Validar que la variedad de producto existe si se proporciona
        if (dto.IdVariedadProducto.HasValue)
        {
            var variedadProducto = await _unitOfWork.Repository<VariedadProducto>()
                .GetByIdAsync(dto.IdVariedadProducto.Value, cancellationToken);
            
            if (variedadProducto == null)
                throw new NotFoundException("VariedadProducto", dto.IdVariedadProducto.Value);
        }

        // Calcular PesoTotal: SacosTotales * PesoPorSaco (50 kg)
        decimal? pesoTotal = null;
        if (dto.SacosTotales.HasValue)
        {
            pesoTotal = dto.SacosTotales.Value * PESO_POR_SACO_DEFAULT;
        }

        // Calcular MontoTotalPago: SacosTotales * PrecioUnitario * PesoPorSaco (50 kg)
        decimal? montoTotalPago = null;
        if (dto.SacosTotales.HasValue && dto.PrecioUnitario.HasValue)
        {
            montoTotalPago = dto.SacosTotales.Value * dto.PrecioUnitario.Value * PESO_POR_SACO_DEFAULT;
        }

        // Actualizar negociación - PRIMERA ETAPA (igual que CREATE)
        negociacion.IdComisionista = dto.IdComisionista;
        negociacion.IdVariedadProducto = dto.IdVariedadProducto;
        negociacion.SacosTotales = dto.SacosTotales;
        negociacion.TipoCalidad = dto.TipoCalidad;
        negociacion.PrecioUnitario = dto.PrecioUnitario;
        negociacion.PesoPorSaco = PESO_POR_SACO_DEFAULT; // Asignar peso por saco por defecto (50 kg)
        negociacion.PesoTotal = pesoTotal; // Asignar peso total calculado
        negociacion.MontoTotalPago = montoTotalPago; // Asignar monto total calculado

        // Mantener el estado en 'EN PROCESO' y EstadoAprobacionIngeniero en 'PENDIENTE'
        negociacion.Estado = "EN PROCESO";
        negociacion.EstadoAprobacionIngeniero = "PENDIENTE";

        await _unitOfWork.Repository<Negociacion>().UpdateAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        negociacion.Comisionista = comisionista;

        if (dto.IdVariedadProducto.HasValue)
        {
            negociacion.VariedadProducto = await _unitOfWork.Repository<VariedadProducto>()
                .GetByIdAsync(dto.IdVariedadProducto.Value, cancellationToken);
        }

        return _mapper.Map<NegociacionDto>(negociacion);
    }
}
