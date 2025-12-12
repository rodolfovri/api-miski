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
        if (!string.IsNullOrEmpty(request.Estado))
        {
            llegadasPlanta = llegadasPlanta.Where(lp => lp.Estado == request.Estado).ToList();
        }

        if (request.FechaDesde.HasValue)
        {
            var fechaDesde = request.FechaDesde.Value;
            llegadasPlanta = llegadasPlanta.Where(lp => lp.FLlegada >= fechaDesde).ToList();
        }

        if (request.FechaHasta.HasValue)
        {
            var fechaHasta = request.FechaHasta.Value.Date.AddDays(1).AddTicks(-1);
            llegadasPlanta = llegadasPlanta.Where(lp => lp.FLlegada <= fechaHasta).ToList();
        }

        // Obtener todas las entidades necesarias
        var todasLasCompras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var todasLasPersonas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var todosLosLotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
        var todasLasUbicaciones = await _unitOfWork.Repository<Ubicacion>().GetAllAsync(cancellationToken);
        var todasLasNegociaciones = await _unitOfWork.Repository<Negociacion>().GetAllAsync(cancellationToken);
        var todasLasVariedadesProducto = await _unitOfWork.Repository<VariedadProducto>().GetAllAsync(cancellationToken);
        var todosLosProductos = await _unitOfWork.Repository<Producto>().GetAllAsync(cancellationToken);
        
        // ? Cargar CompraVehiculo, CompraVehiculoDetalle y Vehiculos para obtener conductor y placa
        var todosLosCompraVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>().GetAllAsync(cancellationToken);
        var todosLosCompraVehiculos = await _unitOfWork.Repository<CompraVehiculo>().GetAllAsync(cancellationToken);
        var todosLosVehiculos = await _unitOfWork.Repository<Vehiculo>().GetAllAsync(cancellationToken);

        // Construir los DTOs
        var resultado = new List<LlegadaPlantaDto>();

        foreach (var llegada in llegadasPlanta)
        {
            // Buscar la compra
            var compra = todasLasCompras.FirstOrDefault(c => c.IdCompra == llegada.IdCompra);

            // Buscar el usuario que recepcionó
            var usuario = todasLasPersonas.FirstOrDefault(p => p.IdPersona == llegada.IdUsuario);

            // Buscar el lote
            var lote = todosLosLotes.FirstOrDefault(l => l.IdLote == llegada.IdLote);

            // Buscar la ubicación
            var ubicacion = todasLasUbicaciones.FirstOrDefault(u => u.IdUbicacion == llegada.IdUbicacion);

            // Buscar el usuario que anuló (si existe)
            Persona? usuarioAnulacion = null;
            if (compra?.IdUsuarioAnulacion.HasValue == true)
            {
                usuarioAnulacion = todasLasPersonas.FirstOrDefault(p => p.IdPersona == compra.IdUsuarioAnulacion.Value);
            }

            // Buscar la Negociación asociada a la Compra
            Negociacion? negociacion = null;
            if (compra != null)
            {
                negociacion = todasLasNegociaciones.FirstOrDefault(n => n.IdNegociacion == compra.IdNegociacion);
            }

            // Buscar la VariedadProducto desde la Negociación
            VariedadProducto? variedadProducto = null;
            if (negociacion?.IdVariedadProducto.HasValue == true)
            {
                variedadProducto = todasLasVariedadesProducto.FirstOrDefault(vp => vp.IdVariedadProducto == negociacion.IdVariedadProducto.Value);
            }

            // Buscar el Producto desde la VariedadProducto
            Producto? producto = null;
            if (variedadProducto != null)
            {
                producto = todosLosProductos.FirstOrDefault(p => p.IdProducto == variedadProducto.IdProducto);
            }

            // ? Buscar el CompraVehiculo y Vehiculo asociado a esta compra
            string? conductorNombre = null;
            string? vehiculoPlaca = null;
            
            if (compra != null)
            {
                // Buscar el detalle de CompraVehiculo para esta compra
                var compraVehiculoDetalle = todosLosCompraVehiculoDetalles
                    .FirstOrDefault(cvd => cvd.IdCompra == compra.IdCompra);
                
                if (compraVehiculoDetalle != null)
                {
                    // Buscar el CompraVehiculo
                    var compraVehiculo = todosLosCompraVehiculos
                        .FirstOrDefault(cv => cv.IdCompraVehiculo == compraVehiculoDetalle.IdCompraVehiculo);
                    
                    if (compraVehiculo != null)
                    {
                        // Buscar el conductor (Persona)
                        var conductor = todasLasPersonas.FirstOrDefault(p => p.IdPersona == compraVehiculo.IdPersona);
                        if (conductor != null)
                        {
                            conductorNombre = $"{conductor.Nombres} {conductor.Apellidos}";
                        }
                        
                        // Buscar el vehículo
                        var vehiculo = todosLosVehiculos.FirstOrDefault(v => v.IdVehiculo == compraVehiculo.IdVehiculo);
                        if (vehiculo != null)
                        {
                            vehiculoPlaca = vehiculo.Placa;
                        }
                    }
                }
            }

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
                IdUbicacion = llegada.IdUbicacion,
                SacosRecibidos = (decimal)llegada.SacosRecibidos,
                PesoRecibido = (decimal)llegada.PesoRecibido,
                FLlegada = llegada.FLlegada,
                Observaciones = llegada.Observaciones,
                Estado = llegada.Estado,
                CompraSerie = compra?.Serie,
                UsuarioNombre = usuario != null ? $"{usuario.Nombres} {usuario.Apellidos}" : null,
                LoteCodigo = lote?.Codigo,
                UbicacionNombre = ubicacion?.Nombre,
                SacosAsignados = lote?.Sacos ?? 0,
                PesoAsignado = lote?.Peso ?? 0,
                DiferenciaSacos = diferenciaSacos,
                DiferenciaPeso = diferenciaPeso,
                // Datos de anulación de la Compra
                IdUsuarioAnulacion = compra?.IdUsuarioAnulacion,
                UsuarioAnulacionNombre = usuarioAnulacion != null ? $"{usuarioAnulacion.Nombres} {usuarioAnulacion.Apellidos}" : null,
                MotivoAnulacion = compra?.MotivoAnulacion,
                FAnulacion = compra?.FAnulacion,
                // Datos del Producto y VariedadProducto
                IdVariedadProducto = variedadProducto?.IdVariedadProducto,
                VariedadProductoNombre = variedadProducto?.Nombre,
                VariedadProductoCodigo = variedadProducto?.Codigo,
                IdProducto = producto?.IdProducto,
                ProductoNombre = producto?.Nombre,
                // ? Datos del conductor y vehículo
                ConductorNombre = conductorNombre,
                VehiculoPlaca = vehiculoPlaca
            });
        }

        // Ordenar por fecha de llegada descendente
        return resultado.OrderByDescending(r => r.FLlegada).ToList();
    }
}
