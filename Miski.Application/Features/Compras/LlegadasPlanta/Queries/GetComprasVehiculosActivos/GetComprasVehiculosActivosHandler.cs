using MediatR;
using Microsoft.EntityFrameworkCore;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Infrastructure.Data;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetComprasVehiculosActivos;

public class GetComprasVehiculosActivosHandler : IRequestHandler<GetComprasVehiculosActivosQuery, List<CompraVehiculoResumenDto>>
{
    private readonly MiskiDbContext _context;

    public GetComprasVehiculosActivosHandler(MiskiDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompraVehiculoResumenDto>> Handle(GetComprasVehiculosActivosQuery request, CancellationToken cancellationToken)
    {
        // ? Obtener todos los CompraVehiculo con Include para cargar relaciones (relación 1:1)
        var comprasVehiculos = await _context.CompraVehiculos
            .Include(cv => cv.Vehiculo)
            .Include(cv => cv.CompraVehiculoDetalles)
                .ThenInclude(cvd => cvd.Compra)
                    .ThenInclude(c => c.Lote)  // ? Lote (singular, relación 1:1)
            .ToListAsync(cancellationToken);

        var resultado = new List<CompraVehiculoResumenDto>();

        foreach (var compraVehiculo in comprasVehiculos)
        {
            // Filtrar solo compras ACTIVAS
            var comprasActivas = compraVehiculo.CompraVehiculoDetalles
                .Where(cvd => cvd.Compra.Estado == "ACTIVO")
                .ToList();

            if (!comprasActivas.Any())
                continue;

            var detalles = new List<CompraDetalleDto>();

            foreach (var cvDetalle in comprasActivas)
            {
                var compra = cvDetalle.Compra;
                
                // ? Obtener el lote asociado a esta compra (relación 1:1)
                var lote = compra.Lote;
                
                if (lote != null)
                {
                    // Buscar si hay llegada a planta para este lote
                    var llegadaPlanta = await _context.LlegadasPlanta
                        .Where(lp => lp.IdLote == lote.IdLote)
                        .FirstOrDefaultAsync(cancellationToken);

                    detalles.Add(new CompraDetalleDto
                    {
                        IdCompra = compra.IdCompra,
                        CodigoLote = lote.Codigo,
                        SacosEnviados = lote.Sacos,
                        PesoEnviado = lote.Peso,
                        SacosRecibidos = llegadaPlanta != null ? (decimal)llegadaPlanta.SacosRecibidos : null,
                        PesoRecibido = llegadaPlanta != null ? (decimal)llegadaPlanta.PesoRecibido : null,
                        EstadoCompra = compra.Estado
                    });
                }
            }

            if (detalles.Any())
            {
                resultado.Add(new CompraVehiculoResumenDto
                {
                    IdCompraVehiculo = compraVehiculo.IdCompraVehiculo,
                    VehiculoPlaca = compraVehiculo.Vehiculo?.Placa,
                    GuiaRemision = compraVehiculo.GuiaRemision,
                    FRegistro = compraVehiculo.FRegistro,
                    Detalles = detalles
                });
            }
        }

        return resultado.OrderByDescending(r => r.FRegistro).ToList();
    }
}
