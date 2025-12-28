using MediatR;
using FluentValidation;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Commands.CreateLlegadaPlanta;

public class CreateLlegadaPlantaHandler : IRequestHandler<CreateLlegadaPlantaCommand, CreateLlegadaPlantaResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateLlegadaPlantaDto> _validator;

    public CreateLlegadaPlantaHandler(IUnitOfWork unitOfWork, IValidator<CreateLlegadaPlantaDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<CreateLlegadaPlantaResponseDto> Handle(CreateLlegadaPlantaCommand request, CancellationToken cancellationToken)
    {
        // Validar el DTO
        var validationResult = await _validator.ValidateAsync(request.Data, cancellationToken);
        if (!validationResult.IsValid)
        {
            // Concatenar todos los errores en un solo mensaje
            var errorMessages = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            
            var errorMessage = string.Join("; ", errorMessages);
            throw new Shared.Exceptions.ValidationException(errorMessage);
        }

        // Verificar que el CompraVehiculo existe
        var compraVehiculo = await _unitOfWork.Repository<CompraVehiculo>()
            .GetByIdAsync(request.Data.IdCompraVehiculo, cancellationToken);

        if (compraVehiculo == null)
            throw new NotFoundException("CompraVehiculo", request.Data.IdCompraVehiculo);

        // Verificar que el usuario existe
        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(request.Data.IdUsuario, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", request.Data.IdUsuario);

        // Cargar la persona asociada al usuario para el nombre
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(usuario.IdUsuario, cancellationToken);

        // Obtener todos los lotes para validaciones
        var todosLosLotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
        var todasLasCompras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var llegadasExistentes = await _unitOfWork.Repository<LlegadaPlanta>().GetAllAsync(cancellationToken);
        var todasLasUbicaciones = await _unitOfWork.Repository<Ubicacion>().GetAllAsync(cancellationToken);

        // Validar cada detalle
        foreach (var detalle in request.Data.Detalles)
        {
            // Verificar que la compra existe
            var compra = todasLasCompras.FirstOrDefault(c => c.IdCompra == detalle.IdCompra);
            if (compra == null)
                throw new NotFoundException("Compra", detalle.IdCompra);

            // Verificar que el lote existe
            var lote = todosLosLotes.FirstOrDefault(l => l.IdLote == detalle.IdLote);
            if (lote == null)
                throw new NotFoundException("Lote", detalle.IdLote);

            // ✅ Verificar que el lote está asignado a la compra (relación 1:1)
            if (compra.IdLote != detalle.IdLote)
            {
                throw new Shared.Exceptions.ValidationException($"El lote con ID {detalle.IdLote} no está asignado a la compra {detalle.IdCompra}");
            }

            // Verificar que la ubicación existe
            var ubicacion = todasLasUbicaciones.FirstOrDefault(u => u.IdUbicacion == detalle.IdUbicacion);
            if (ubicacion == null)
                throw new NotFoundException("Ubicacion", detalle.IdUbicacion);

            // Verificar que el lote no haya sido registrado previamente
            var loteYaRegistrado = llegadasExistentes.Any(lp => lp.IdLote == detalle.IdLote);
            if (loteYaRegistrado)
            {
                throw new Shared.Exceptions.ValidationException($"El lote con ID {detalle.IdLote} ya ha sido registrado en una llegada a planta");
            }
        }

        // Fecha de llegada automática
        var fechaLlegada = DateTime.UtcNow;

        var llegadasRegistradas = new List<LlegadaPlantaDto>();
        var comprasActualizadas = new HashSet<int>(); // Para rastrear qué compras debemos actualizar
        var llegadasPlantaCreadas = new List<LlegadaPlanta>(); // Para crear MovimientoAlmacen después

        // Diccionario para acumular peso y sacos por compra Y ubicación (para actualizar Stock)
        // Clave: (IdCompra, IdUbicacion), Valor: (PesoTotal, SacosTotales)
        var pesoYSacosPorCompraYUbicacion = new Dictionary<(int, int), (decimal peso, int sacos)>();

        // Crear las llegadas a planta para cada detalle
        foreach (var detalleDto in request.Data.Detalles)
        {
            var lote = todosLosLotes.First(l => l.IdLote == detalleDto.IdLote);
            var compra = todasLasCompras.First(c => c.IdCompra == detalleDto.IdCompra);
            var ubicacion = todasLasUbicaciones.First(u => u.IdUbicacion == detalleDto.IdUbicacion);

            // Crear la llegada a planta (usar IdUbicacion del detalle)
            var llegadaPlanta = new LlegadaPlanta
            {
                IdCompra = detalleDto.IdCompra,
                IdUsuario = request.Data.IdUsuario,
                IdLote = detalleDto.IdLote,
                IdUbicacion = detalleDto.IdUbicacion,
                SacosRecibidos = (double)detalleDto.SacosRecibidos,
                PesoRecibido = (double)detalleDto.PesoRecibido,
                FLlegada = fechaLlegada,
                Observaciones = detalleDto.Observaciones,
                Estado = "RECEPCIONADO"
            };

            await _unitOfWork.Repository<LlegadaPlanta>().AddAsync(llegadaPlanta, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Guardar la llegada para crear MovimientoAlmacen después
            llegadasPlantaCreadas.Add(llegadaPlanta);

            // Agregar compra a la lista de compras a actualizar
            comprasActualizadas.Add(detalleDto.IdCompra);

            // Acumular peso y sacos por compra Y ubicación para actualizar Stock
            var key = (detalleDto.IdCompra, detalleDto.IdUbicacion);
            if (!pesoYSacosPorCompraYUbicacion.ContainsKey(key))
            {
                pesoYSacosPorCompraYUbicacion[key] = (0, 0);
            }
            var valorActual = pesoYSacosPorCompraYUbicacion[key];
            pesoYSacosPorCompraYUbicacion[key] = (valorActual.peso + detalleDto.PesoRecibido, valorActual.sacos + (int)detalleDto.SacosRecibidos);

            // Calcular diferencias
            int diferenciaSacos = lote.Sacos - (int)llegadaPlanta.SacosRecibidos;
            decimal diferenciaPeso = lote.Peso - (decimal)llegadaPlanta.PesoRecibido;

            // Agregar a la respuesta
            llegadasRegistradas.Add(new LlegadaPlantaDto
            {
                IdLlegadaPlanta = llegadaPlanta.IdLlegadaPlanta,
                IdCompra = llegadaPlanta.IdCompra,
                IdUsuario = llegadaPlanta.IdUsuario,
                IdLote = llegadaPlanta.IdLote,
                IdUbicacion = llegadaPlanta.IdUbicacion,
                SacosRecibidos = (decimal)llegadaPlanta.SacosRecibidos,
                PesoRecibido = (decimal)llegadaPlanta.PesoRecibido,
                FLlegada = llegadaPlanta.FLlegada,
                Observaciones = llegadaPlanta.Observaciones,
                Estado = llegadaPlanta.Estado,
                CompraSerie = compra.Serie,
                UsuarioNombre = persona != null ? $"{persona.Nombres} {persona.Apellidos}" : "Usuario desconocido",
                LoteCodigo = lote.Codigo,
                UbicacionNombre = ubicacion.Nombre,
                SacosAsignados = lote.Sacos,
                PesoAsignado = lote.Peso,
                DiferenciaSacos = diferenciaSacos,
                DiferenciaPeso = diferenciaPeso
            });
        }

        // Actualizar el EstadoRecepcion de las compras recepcionadas
        foreach (var idCompra in comprasActualizadas)
        {
            var compra = todasLasCompras.First(c => c.IdCompra == idCompra);
            compra.EstadoRecepcion = "RECEPCIONADO";
            await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar negociaciones para MovimientoAlmacen y Stock
        var todasLasNegociaciones = await _unitOfWork.Repository<Negociacion>().GetAllAsync(cancellationToken);

        // ✅ CREAR MovimientoAlmacen por cada ubicación procesada
        // Agrupar las llegadas por ubicación para crear un MovimientoAlmacen por ubicación
        var llegadasPorUbicacion = llegadasPlantaCreadas
            .GroupBy(lp => lp.IdUbicacion)
            .ToList();

        foreach (var grupo in llegadasPorUbicacion)
        {
            var idUbicacion = grupo.Key;
            var llegadasDeEstaUbicacion = grupo.ToList();

            // Crear el MovimientoAlmacen
            var movimientoAlmacen = new MovimientoAlmacen
            {
                IdTipoMovimiento = 1, // ID 1 = INGRESO (según instrucciones)
                IdUbicacion = idUbicacion,
                TipoStock = "MATERIA_PRIMA",
                IdLlegadaPlanta = llegadasDeEstaUbicacion.First().IdLlegadaPlanta, // Asociar con la primera llegada del grupo
                IdUsuario = request.Data.IdUsuario,
                FRegistro = DateTime.UtcNow,
                Observaciones = $"Ingreso automático por recepción de {llegadasDeEstaUbicacion.Count} lote(s)",
                Estado = "ACTIVO"
            };

            await _unitOfWork.Repository<MovimientoAlmacen>().AddAsync(movimientoAlmacen, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Crear los DetalleMovimientoAlmacen para cada llegada
            foreach (var llegada in llegadasDeEstaUbicacion)
            {
                // Obtener la compra para acceder a la negociación y obtener IdVariedadProducto
                var compra = todasLasCompras.First(c => c.IdCompra == llegada.IdCompra);
                var negociacion = todasLasNegociaciones.FirstOrDefault(n => n.IdNegociacion == compra.IdNegociacion);

                if (negociacion == null || !negociacion.IdVariedadProducto.HasValue)
                {
                    // Si no hay negociación o no tiene variedad de producto, saltar
                    continue;
                }

                var detalleMovimiento = new DetalleMovimientoAlmacen
                {
                    IdMovimientoAlmacen = movimientoAlmacen.IdMovimientoAlmacen,
                    IdVariedadProducto = negociacion.IdVariedadProducto.Value,
                    IdLote = llegada.IdLote,
                    Cantidad = (decimal)llegada.PesoRecibido,
                    NumeroSacos = (int)llegada.SacosRecibidos
                };

                await _unitOfWork.Repository<DetalleMovimientoAlmacen>().AddAsync(detalleMovimiento, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Actualizar o crear Stock por cada compra Y ubicación procesada
        var todosLosStocks = await _unitOfWork.Repository<Stock>().GetAllAsync(cancellationToken);

        foreach (var kvp in pesoYSacosPorCompraYUbicacion)
        {
            var (idCompra, idUbicacion) = kvp.Key;
            var (pesoTotal, sacosTotal) = kvp.Value;

            var compra = todasLasCompras.First(c => c.IdCompra == idCompra);
            var negociacion = todasLasNegociaciones.FirstOrDefault(n => n.IdNegociacion == compra.IdNegociacion);

            if (negociacion == null || !negociacion.IdVariedadProducto.HasValue)
            {
                // Si no hay negociación o no tiene variedad de producto, no podemos actualizar stock
                continue;
            }

            var idVariedadProducto = negociacion.IdVariedadProducto.Value;

            // Buscar si ya existe un Stock para esta combinación de Planta + VariedadProducto
            var stockExistente = todosLosStocks.FirstOrDefault(s => 
                s.IdPlanta == idUbicacion && 
                s.IdVariedadProducto == idVariedadProducto);

            if (stockExistente != null)
            {
                // Actualizar stock existente (sumar el peso y sacos recibidos)
                stockExistente.CantidadKg = (stockExistente.CantidadKg ?? 0) + pesoTotal;
                stockExistente.CantidadSacos = (stockExistente.CantidadSacos ?? 0) + sacosTotal;
                await _unitOfWork.Repository<Stock>().UpdateAsync(stockExistente, cancellationToken);
            }
            else
            {
                // Crear nuevo stock
                var nuevoStock = new Stock
                {
                    IdVariedadProducto = idVariedadProducto,
                    IdPlanta = idUbicacion,
                    CantidadKg = pesoTotal,
                    CantidadSacos = sacosTotal,
                    TipoStock = "MATERIA_PRIMA"
                };
                await _unitOfWork.Repository<Stock>().AddAsync(nuevoStock, cancellationToken);
            }
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cambiar el estado del CompraVehiculo a ENTREGADO
        compraVehiculo.Estado = "ENTREGADO";
        await _unitOfWork.Repository<CompraVehiculo>().UpdateAsync(compraVehiculo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ✅ NUEVA LÓGICA: Determinar el estado del CompraVehiculo
        // Obtener todas las compras asociadas al CompraVehiculo
        var compraVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>().GetAllAsync(cancellationToken);
        var comprasDelVehiculo = compraVehiculoDetalles
            .Where(cvd => cvd.IdCompraVehiculo == request.Data.IdCompraVehiculo)
            .Select(cvd => cvd.IdCompra)
            .ToList();

        // Verificar cuántas compras del vehículo han sido recepcionadas
        var comprasRecepcionadas = todasLasCompras
            .Where(c => comprasDelVehiculo.Contains(c.IdCompra) && c.EstadoRecepcion == "RECEPCIONADO")
            .Count();

        var totalComprasDelVehiculo = comprasDelVehiculo.Count;

        // Determinar el estado del CompraVehiculo
        if (comprasRecepcionadas == totalComprasDelVehiculo)
        {
            // Todas las compras han sido recepcionadas
            compraVehiculo.Estado = "ENTREGADO";
        }
        else if (comprasRecepcionadas > 0)
        {
            // Solo algunas compras han sido recepcionadas
            compraVehiculo.Estado = "PARCIAL";
        }
        // Si comprasRecepcionadas == 0, mantener el estado actual (no debería pasar si estamos registrando llegadas)

        await _unitOfWork.Repository<CompraVehiculo>().UpdateAsync(compraVehiculo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Construir el DTO de respuesta
        var resultado = new CreateLlegadaPlantaResponseDto
        {
            IdCompraVehiculo = request.Data.IdCompraVehiculo,
            TotalLotesRecibidos = llegadasRegistradas.Count,
            FLlegada = fechaLlegada,
            LlegadasRegistradas = llegadasRegistradas
        };

        return resultado;
    }
}
