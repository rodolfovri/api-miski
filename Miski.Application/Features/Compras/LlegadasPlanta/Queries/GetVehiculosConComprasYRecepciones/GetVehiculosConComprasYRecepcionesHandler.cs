using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetVehiculosConComprasYRecepciones;

public class GetVehiculosConComprasYRecepcionesHandler : IRequestHandler<GetVehiculosConComprasYRecepcionesQuery, List<VehiculoConComprasYRecepcionesDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehiculosConComprasYRecepcionesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<VehiculoConComprasYRecepcionesDto>> Handle(GetVehiculosConComprasYRecepcionesQuery request, CancellationToken cancellationToken)
    {
        // Obtener todos los CompraVehiculo
        var todosLosCompraVehiculos = await _unitOfWork.Repository<CompraVehiculo>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtros si se proporcionan
        if (request.IdCompraVehiculo.HasValue)
        {
            todosLosCompraVehiculos = todosLosCompraVehiculos
                .Where(cv => cv.IdCompraVehiculo == request.IdCompraVehiculo.Value)
                .ToList();
        }

        if (request.IdVehiculo.HasValue)
        {
            todosLosCompraVehiculos = todosLosCompraVehiculos
                .Where(cv => cv.IdVehiculo == request.IdVehiculo.Value)
                .ToList();
        }

        // Obtener todas las entidades necesarias
        var todosLosVehiculos = await _unitOfWork.Repository<Vehiculo>().GetAllAsync(cancellationToken);
        var todasLasPersonas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var todosLosDetallesCV = await _unitOfWork.Repository<CompraVehiculoDetalle>().GetAllAsync(cancellationToken);
        var todasLasCompras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var todosLosLotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
        var todasLasLlegadas = await _unitOfWork.Repository<LlegadaPlanta>().GetAllAsync(cancellationToken);

        var resultado = new List<VehiculoConComprasYRecepcionesDto>();

        foreach (var compraVehiculo in todosLosCompraVehiculos)
        {
            // Obtener el vehículo
            var vehiculo = todosLosVehiculos.FirstOrDefault(v => v.IdVehiculo == compraVehiculo.IdVehiculo);

            // Obtener la persona
            var persona = todasLasPersonas.FirstOrDefault(p => p.IdPersona == compraVehiculo.IdPersona);

            // Obtener las compras asignadas a este vehículo
            var detallesCV = todosLosDetallesCV
                .Where(dcv => dcv.IdCompraVehiculo == compraVehiculo.IdCompraVehiculo)
                .ToList();

            var comprasDto = new List<CompraConRecepcionDetalladaDto>();

            foreach (var detalleCV in detallesCV)
            {
                var compra = todasLasCompras.FirstOrDefault(c => c.IdCompra == detalleCV.IdCompra);
                
                if (compra != null)
                {
                    // Obtener el lote de esta compra (relación 1:1)
                    var lotesDto = new List<LoteConRecepcionDetalladoDto>();
                    
                    if (compra.IdLote.HasValue)
                    {
                        var lote = todosLosLotes.FirstOrDefault(l => l.IdLote == compra.IdLote.Value);
                        
                        if (lote != null)
                        {
                            // Buscar si este lote tiene registro en LlegadaPlanta
                            var llegadaPlanta = todasLasLlegadas
                                .FirstOrDefault(lp => lp.IdLote == lote.IdLote);

                            int? diferenciaSacos = null;
                            decimal? diferenciaPeso = null;

                            if (llegadaPlanta != null)
                            {
                                diferenciaSacos = lote.Sacos - (int)llegadaPlanta.SacosRecibidos;
                                diferenciaPeso = lote.Peso - (decimal)llegadaPlanta.PesoRecibido;
                            }

                            lotesDto.Add(new LoteConRecepcionDetalladoDto
                            {
                                IdLote = lote.IdLote,
                                Codigo = lote.Codigo,
                                SacosAsignados = lote.Sacos,
                                PesoAsignado = lote.Peso,
                                IdLlegadaPlanta = llegadaPlanta?.IdLlegadaPlanta,
                                SacosRecibidos = llegadaPlanta != null ? (decimal)llegadaPlanta.SacosRecibidos : null,
                                PesoRecibido = llegadaPlanta != null ? (decimal)llegadaPlanta.PesoRecibido : null,
                                DiferenciaSacos = diferenciaSacos,
                                DiferenciaPeso = diferenciaPeso,
                                Observaciones = llegadaPlanta?.Observaciones,
                                YaRecibido = llegadaPlanta != null
                            });
                        }
                    }

                    comprasDto.Add(new CompraConRecepcionDetalladaDto
                    {
                        IdCompra = compra.IdCompra,
                        Serie = compra.Serie,
                        FRegistro = compra.FRegistro,
                        Estado = compra.Estado,
                        Lotes = lotesDto
                    });
                }
            }

            resultado.Add(new VehiculoConComprasYRecepcionesDto
            {
                IdCompraVehiculo = compraVehiculo.IdCompraVehiculo,
                IdPersona = compraVehiculo.IdPersona,
                IdVehiculo = compraVehiculo.IdVehiculo,
                GuiaRemision = compraVehiculo.GuiaRemision,
                FRegistro = compraVehiculo.FRegistro,
                Estado = compraVehiculo.Estado,
                PersonaNombre = persona != null ? $"{persona.Nombres} {persona.Apellidos}" : string.Empty,
                VehiculoPlaca = vehiculo?.Placa,
                VehiculoMarca = vehiculo?.Marca,
                VehiculoModelo = vehiculo?.Modelo,
                Compras = comprasDto
            });
        }

        // Ordenar por fecha de registro descendente
        return resultado.OrderByDescending(r => r.FRegistro).ToList();
    }
}
