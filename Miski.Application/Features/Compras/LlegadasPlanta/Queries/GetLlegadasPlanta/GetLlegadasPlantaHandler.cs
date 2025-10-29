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

            // Buscar el lote
            var lote = todosLosLotes.FirstOrDefault(l => l.IdLote == llegada.IdLote);

            // Calcular diferencias
            int diferenciaSacos = 0;
            decimal diferenciaPeso = 0;

            if (lote != null)
            {
                diferenciaSacos = lote.Sacos - (int)llegada.SacosRecibidos;
                diferenciaPeso = lote.Peso - (decimal)llegada.PesoRecibido;
            }

            resultado.Add(new LlegadaPlantaDto
            {
                IdLlegadaPlanta = llegada.IdLlegadaPlanta,
                IdCompra = llegada.IdCompra,
                IdUsuario = llegada.IdUsuario,
                IdLote = llegada.IdLote,
                SacosRecibidos = (decimal)llegada.SacosRecibidos,
                PesoRecibido = (decimal)llegada.PesoRecibido,
                FLlegada = llegada.FLlegada,
                Observaciones = llegada.Observaciones,
                Estado = llegada.Estado,
                CompraSerie = compra?.Serie,
                UsuarioNombre = usuario != null ? $"{usuario.Nombres} {usuario.Apellidos}" : null,
                LoteCodigo = lote?.Codigo,
                SacosAsignados = lote?.Sacos ?? 0,
                PesoAsignado = lote?.Peso ?? 0,
                DiferenciaSacos = diferenciaSacos,
                DiferenciaPeso = diferenciaPeso
            });
        }

        // Ordenar por fecha de llegada descendente
        return resultado.OrderByDescending(r => r.FLlegada).ToList();
    }
}
