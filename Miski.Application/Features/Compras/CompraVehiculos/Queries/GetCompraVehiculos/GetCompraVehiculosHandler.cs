using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculos;

public class GetCompraVehiculosHandler : IRequestHandler<GetCompraVehiculosQuery, IEnumerable<CompraVehiculoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompraVehiculosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompraVehiculoDto>> Handle(GetCompraVehiculosQuery request, CancellationToken cancellationToken)
    {
        var comprasVehiculos = await _unitOfWork.Repository<CompraVehiculo>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (request.FechaDesde.HasValue)
        {
            var fechaDesde = request.FechaDesde.Value.Date;
            comprasVehiculos = comprasVehiculos
                .Where(cv => cv.FRegistro >= fechaDesde)
                .ToList();
        }

        if (request.FechaHasta.HasValue)
        {
            var fechaHasta = request.FechaHasta.Value.Date;
            comprasVehiculos = comprasVehiculos
                .Where(cv => cv.FRegistro <= fechaHasta)
                .ToList();
        }

        // Cargar relaciones
        var comprasVehiculosList = comprasVehiculos.ToList();
        
        foreach (var compraVehiculo in comprasVehiculosList)
        {
            // Cargar persona
            compraVehiculo.Persona = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(compraVehiculo.IdPersona, cancellationToken) ?? new Persona();

            // Cargar vehículo
            compraVehiculo.Vehiculo = await _unitOfWork.Repository<Vehiculo>()
                .GetByIdAsync(compraVehiculo.IdVehiculo, cancellationToken) ?? new Vehiculo();

            // Cargar detalles
            var detalles = await _unitOfWork.Repository<CompraVehiculoDetalle>()
                .GetAllAsync(cancellationToken);
            
            compraVehiculo.CompraVehiculoDetalles = detalles
                .Where(d => d.IdCompraVehiculo == compraVehiculo.IdCompraVehiculo)
                .ToList();

            // Cargar compras y lotes en los detalles
            foreach (var detalle in compraVehiculo.CompraVehiculoDetalles)
            {
                detalle.Compra = await _unitOfWork.Repository<Compra>()
                    .GetByIdAsync(detalle.IdCompra, cancellationToken) ?? new Compra();
                
                // ? Cargar el lote de la compra (relación 1:1)
                if (detalle.Compra != null && detalle.Compra.IdLote.HasValue)
                {
                    detalle.Compra.Lote = await _unitOfWork.Repository<Lote>()
                        .GetByIdAsync(detalle.Compra.IdLote.Value, cancellationToken);
                }
            }
        }

        return _mapper.Map<IEnumerable<CompraVehiculoDto>>(comprasVehiculosList);
    }
}
