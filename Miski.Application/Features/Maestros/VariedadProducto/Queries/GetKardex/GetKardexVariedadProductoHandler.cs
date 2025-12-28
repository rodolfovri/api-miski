using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.VariedadProducto.Queries.GetKardex;

public class GetKardexVariedadProductoHandler : IRequestHandler<GetKardexVariedadProductoQuery, KardexVariedadProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetKardexVariedadProductoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<KardexVariedadProductoDto> Handle(GetKardexVariedadProductoQuery request, CancellationToken cancellationToken)
    {
        // 1. Verificar que la variedad de producto existe
        var variedadProducto = await _unitOfWork.Repository<Domain.Entities.VariedadProducto>()
            .GetByIdAsync(request.IdVariedadProducto, cancellationToken);

        if (variedadProducto == null)
            throw new NotFoundException("VariedadProducto", request.IdVariedadProducto);

        // 2. Cargar el producto asociado
        var producto = await _unitOfWork.Repository<Producto>()
            .GetByIdAsync(variedadProducto.IdProducto, cancellationToken);

        // 3. Obtener todos los movimientos de almacén
        var todosLosMovimientos = await _unitOfWork.Repository<MovimientoAlmacen>().GetAllAsync(cancellationToken);
        var todosLosDetalles = await _unitOfWork.Repository<DetalleMovimientoAlmacen>().GetAllAsync(cancellationToken);
        var todosLosTiposMovimiento = await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>().GetAllAsync(cancellationToken);
        var todasLasUbicaciones = await _unitOfWork.Repository<Ubicacion>().GetAllAsync(cancellationToken);
        var todosLosUsuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        var todasLasPersonas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var todosLosLotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);

        // Ajustar las fechas para incluir todo el día
        var fechaDesde = request.FechaDesde.Date; // 00:00:00
        var fechaHasta = request.FechaHasta.Date.AddDays(1).AddTicks(-1); // 23:59:59

        // 4. Filtrar movimientos que afectan esta variedad de producto en el rango de fechas
        var movimientosRelevantes = todosLosMovimientos
            .Where(m => m.FRegistro >= fechaDesde && m.FRegistro <= fechaHasta)
            .Where(m => m.Estado == "ACTIVO")
            .ToList();

        // 5. Obtener los detalles de movimientos que corresponden a esta variedad
        var detallesRelevantes = todosLosDetalles
            .Where(d => movimientosRelevantes.Any(m => m.IdMovimientoAlmacen == d.IdMovimientoAlmacen))
            .Where(d => d.IdVariedadProducto == request.IdVariedadProducto)
            .ToList();

        // 6. Calcular stock inicial (movimientos antes de FechaDesde)
        var movimientosAnteriores = todosLosMovimientos
            .Where(m => m.FRegistro < fechaDesde)
            .Where(m => m.Estado == "ACTIVO")
            .ToList();

        var detallesAnteriores = todosLosDetalles
            .Where(d => movimientosAnteriores.Any(m => m.IdMovimientoAlmacen == d.IdMovimientoAlmacen))
            .Where(d => d.IdVariedadProducto == request.IdVariedadProducto)
            .ToList();

        decimal stockInicial = 0;
        int sacosInicial = 0;

        foreach (var detalle in detallesAnteriores)
        {
            var movimiento = movimientosAnteriores.First(m => m.IdMovimientoAlmacen == detalle.IdMovimientoAlmacen);
            var tipoMovimiento = todosLosTiposMovimiento.FirstOrDefault(t => t.IdTipoMovimiento == movimiento.IdTipoMovimiento);

            if (tipoMovimiento?.TipoOperacion == "INGRESO")
            {
                stockInicial += detalle.Cantidad;
                sacosInicial += detalle.NumeroSacos;
            }
            else if (tipoMovimiento?.TipoOperacion == "SALIDA")
            {
                stockInicial -= detalle.Cantidad;
                sacosInicial -= detalle.NumeroSacos;
            }
        }

        // 7. Construir la lista de movimientos con saldo acumulado
        var movimientosKardex = new List<MovimientoKardexDto>();
        decimal saldoActual = stockInicial;
        int sacosActual = sacosInicial;

        // Ordenar por fecha
        var movimientosOrdenados = movimientosRelevantes
            .OrderBy(m => m.FRegistro)
            .ToList();

        foreach (var movimiento in movimientosOrdenados)
        {
            // Buscar si hay detalles de esta variedad en este movimiento
            var detallesDelMovimiento = detallesRelevantes
                .Where(d => d.IdMovimientoAlmacen == movimiento.IdMovimientoAlmacen)
                .ToList();

            if (!detallesDelMovimiento.Any())
                continue; // No hay detalles de esta variedad en este movimiento

            // Sumar cantidades de todos los detalles de este movimiento
            decimal cantidadTotal = detallesDelMovimiento.Sum(d => d.Cantidad);
            int sacosTotal = detallesDelMovimiento.Sum(d => d.NumeroSacos);

            var tipoMovimiento = todosLosTiposMovimiento.FirstOrDefault(t => t.IdTipoMovimiento == movimiento.IdTipoMovimiento);
            var ubicacion = todasLasUbicaciones.FirstOrDefault(u => u.IdUbicacion == movimiento.IdUbicacion);
            var usuario = todosLosUsuarios.FirstOrDefault(u => u.IdUsuario == movimiento.IdUsuario);
            var persona = usuario != null && usuario.IdPersona.HasValue 
                ? todasLasPersonas.FirstOrDefault(p => p.IdPersona == usuario.IdPersona.Value)
                : null;

            // Obtener código de lote (tomar el primer detalle que tenga lote)
            var primerDetalle = detallesDelMovimiento.FirstOrDefault(d => d.IdLote.HasValue);
            var lote = primerDetalle?.IdLote.HasValue == true
                ? todosLosLotes.FirstOrDefault(l => l.IdLote == primerDetalle.IdLote.Value)
                : null;

            decimal cantidadIngreso = 0;
            int sacosIngreso = 0;
            decimal cantidadSalida = 0;
            int sacosSalida = 0;

            if (tipoMovimiento?.TipoOperacion == "INGRESO")
            {
                cantidadIngreso = cantidadTotal;
                sacosIngreso = sacosTotal;
                saldoActual += cantidadTotal;
                sacosActual += sacosTotal;
            }
            else if (tipoMovimiento?.TipoOperacion == "SALIDA")
            {
                cantidadSalida = cantidadTotal;
                sacosSalida = sacosTotal;
                saldoActual -= cantidadTotal;
                sacosActual -= sacosTotal;
            }

            movimientosKardex.Add(new MovimientoKardexDto
            {
                IdMovimientoAlmacen = movimiento.IdMovimientoAlmacen,
                Fecha = movimiento.FRegistro,
                TipoOperacion = tipoMovimiento?.TipoOperacion ?? "DESCONOCIDO",
                Descripcion = tipoMovimiento?.Descripcion ?? "Sin descripción",
                Observaciones = movimiento.Observaciones,
                IdLlegadaPlanta = movimiento.IdLlegadaPlanta,
                LoteCodigo = lote?.Codigo,
                CantidadIngreso = cantidadIngreso,
                SacosIngreso = sacosIngreso,
                CantidadSalida = cantidadSalida,
                SacosSalida = sacosSalida,
                SaldoCantidad = saldoActual,
                SaldoSacos = sacosActual,
                UsuarioNombre = persona != null ? $"{persona.Nombres} {persona.Apellidos}" : "Usuario desconocido",
                UbicacionNombre = ubicacion?.Nombre ?? "Sin ubicación"
            });
        }

        // 8. Construir el DTO de respuesta
        var kardex = new KardexVariedadProductoDto
        {
            IdVariedadProducto = variedadProducto.IdVariedadProducto,
            CodigoVariedad = variedadProducto.Codigo,
            NombreVariedad = variedadProducto.Nombre,
            NombreProducto = producto?.Nombre ?? "Sin producto",
            FechaDesde = request.FechaDesde,
            FechaHasta = request.FechaHasta,
            StockInicial = stockInicial,
            SacosInicial = sacosInicial,
            Movimientos = movimientosKardex,
            StockFinal = saldoActual,
            SacosFinal = sacosActual
        };

        return kardex;
    }
}
