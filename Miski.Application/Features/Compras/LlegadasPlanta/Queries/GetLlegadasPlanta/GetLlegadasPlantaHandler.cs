using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetLlegadasPlanta;

public class GetLlegadasPlantaHandler : IRequestHandler<GetLlegadasPlantaQuery, List<LlegadaPlantaDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLlegadasPlantaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<LlegadaPlantaDto>> Handle(GetLlegadasPlantaQuery request, CancellationToken cancellationToken)
    {
        // Obtener todas las llegadas a planta
        var llegadasPlanta = await _unitOfWork.Repository<LlegadaPlanta>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtros opcionales
        if (request.IdCompra.HasValue)
        {
            llegadasPlanta = llegadasPlanta.Where(lp => lp.IdCompra == request.IdCompra.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            llegadasPlanta = llegadasPlanta.Where(lp => lp.Estado == request.Estado).ToList();
        }

        if (request.FechaInicio.HasValue)
        {
            llegadasPlanta = llegadasPlanta.Where(lp => lp.FLlegada >= request.FechaInicio.Value).ToList();
        }

        if (request.FechaFin.HasValue)
        {
            llegadasPlanta = llegadasPlanta.Where(lp => lp.FLlegada <= request.FechaFin.Value).ToList();
        }

        // Obtener todas las compras
        var todasLasCompras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);

        // Obtener todos los usuarios
        var todasLasPersonas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);

        // Obtener todos los detalles de llegadas
        var todosLosDetalles = await _unitOfWork.Repository<LlegadaPlantaDetalle>()
            .GetAllAsync(cancellationToken);

        // Obtener todos los lotes
        var todosLosLotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);

        // Construir los DTOs
        var resultado = new List<LlegadaPlantaDto>();

        foreach (var llegada in llegadasPlanta)
        {
            // Buscar la compra
            var compra = todasLasCompras.FirstOrDefault(c => c.IdCompra == llegada.IdCompra);

            // Buscar el usuario
            var usuario = todasLasPersonas.FirstOrDefault(p => p.IdPersona == llegada.IdUsuario);

            // Obtener los detalles de esta llegada
            var detallesLlegada = todosLosDetalles
                .Where(d => d.IdLlegadaPlanta == llegada.IdLlegadaPlanta)
                .ToList();

            // Crear lista de detalles DTO
            var detallesDto = new List<LlegadaPlantaDetalleDto>();

            foreach (var detalle in detallesLlegada)
            {
                var lote = todosLosLotes.FirstOrDefault(l => l.IdLote == detalle.IdLote);

                if (lote != null)
                {
                    detallesDto.Add(new LlegadaPlantaDetalleDto
                    {
                        IdLlegadaDetalle = detalle.IdLlegadaDetalle,
                        IdLlegadaPlanta = detalle.IdLlegadaPlanta,
                        IdLote = detalle.IdLote,
                        SacosRecibidos = detalle.SacosRecibidos,
                        PesoRecibido = detalle.PesoRecibido,
                        Observaciones = detalle.Observaciones,
                        LoteCodigo = lote.Codigo,
                        SacosAsignados = lote.Sacos,
                        PesoAsignado = lote.Peso,
                        DiferenciaSacos = lote.Sacos - detalle.SacosRecibidos,
                        DiferenciaPeso = lote.Peso - detalle.PesoRecibido
                    });
                }
            }

            resultado.Add(new LlegadaPlantaDto
            {
                IdLlegadaPlanta = llegada.IdLlegadaPlanta,
                IdCompra = llegada.IdCompra,
                IdUsuario = llegada.IdUsuario,
                FLlegada = llegada.FLlegada,
                Observaciones = llegada.Observaciones,
                Estado = llegada.Estado,
                CompraSerie = compra?.Serie,
                UsuarioNombre = usuario != null ? $"{usuario.Nombres} {usuario.Apellidos}" : null,
                Detalles = detallesDto
            });
        }

        // Ordenar por fecha de llegada descendente
        return resultado.OrderByDescending(r => r.FLlegada).ToList();
    }
}
