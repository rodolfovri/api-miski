using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetCompraVehiculoConLotes;

public class GetCompraVehiculoConLotesHandler : IRequestHandler<GetCompraVehiculoConLotesQuery, CompraVehiculoConLotesDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCompraVehiculoConLotesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CompraVehiculoConLotesDto> Handle(GetCompraVehiculoConLotesQuery request, CancellationToken cancellationToken)
    {
        // Obtener el CompraVehiculo
        var compraVehiculo = await _unitOfWork.Repository<CompraVehiculo>()
            .GetByIdAsync(request.IdCompraVehiculo, cancellationToken);

        if (compraVehiculo == null)
            throw new NotFoundException("CompraVehiculo", request.IdCompraVehiculo);

        // Cargar el veh�culo
        var vehiculo = await _unitOfWork.Repository<Vehiculo>()
            .GetByIdAsync(compraVehiculo.IdVehiculo, cancellationToken);

        // Obtener los detalles (compras asignadas a este veh�culo)
        var comprasVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>()
            .GetAllAsync(cancellationToken);

        var detallesDeEsteVehiculo = comprasVehiculoDetalles
            .Where(cvd => cvd.IdCompraVehiculo == request.IdCompraVehiculo)
            .ToList();

        // Obtener todas las compras
        var todasLasCompras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);

        // Obtener todos los lotes
        var todosLosLotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);

        // Obtener todos los detalles de llegada a planta
        var todosLosDetallesLlegada = await _unitOfWork.Repository<LlegadaPlantaDetalle>()
            .GetAllAsync(cancellationToken);

        // Crear lista de compras con lotes
        var comprasDto = new List<CompraConLotesDto>();

        foreach (var detalle in detallesDeEsteVehiculo)
        {
            var compra = todasLasCompras.FirstOrDefault(c => c.IdCompra == detalle.IdCompra);
            
            if (compra != null)
            {
                // Obtener lotes de esta compra
                var lotesDeCompra = todosLosLotes.Where(l => l.IdCompra == compra.IdCompra).ToList();

                // Crear lista de lotes con informaci�n de recepci�n
                var lotesDto = new List<LoteConRecepcionDto>();

                foreach (var lote in lotesDeCompra)
                {
                    // Buscar si este lote ya tiene registro en LlegadaPlantaDetalle
                    var detalleRecepcion = todosLosDetallesLlegada
                        .FirstOrDefault(dll => dll.IdLote == lote.IdLote);

                    lotesDto.Add(new LoteConRecepcionDto
                    {
                        IdLote = lote.IdLote,
                        Codigo = lote.Codigo,
                        SacosAsignados = lote.Sacos,
                        PesoAsignado = lote.Peso,
                        IdLlegadaDetalle = detalleRecepcion?.IdLlegadaDetalle,
                        SacosRecibidos = detalleRecepcion?.SacosRecibidos,
                        PesoRecibido = detalleRecepcion?.PesoRecibido,
                        Observaciones = detalleRecepcion?.Observaciones,
                        YaRecibido = detalleRecepcion != null
                    });
                }

                comprasDto.Add(new CompraConLotesDto
                {
                    IdCompra = compra.IdCompra,
                    Serie = compra.Serie,
                    FRegistro = compra.FRegistro,
                    Lotes = lotesDto
                });
            }
        }

        // Crear el DTO de respuesta
        var resultado = new CompraVehiculoConLotesDto
        {
            IdCompraVehiculo = compraVehiculo.IdCompraVehiculo,
            IdVehiculo = compraVehiculo.IdVehiculo,
            GuiaRemision = compraVehiculo.GuiaRemision,
            FRegistro = compraVehiculo.FRegistro,
            VehiculoPlaca = vehiculo?.Placa,
            VehiculoMarca = vehiculo?.Marca,
            VehiculoModelo = vehiculo?.Modelo,
            Compras = comprasDto
        };

        return resultado;
    }
}
