using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.CompraVehiculos.Commands.CreateCompraVehiculo;

public class CreateCompraVehiculoHandler : IRequestHandler<CreateCompraVehiculoCommand, CompraVehiculoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompraVehiculoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CompraVehiculoDto> Handle(CreateCompraVehiculoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CompraVehiculo;

        // Validar que la persona existe
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(dto.IdPersona, cancellationToken);
        
        if (persona == null)
            throw new NotFoundException("Persona", dto.IdPersona);

        // Validar que el vehículo existe
        var vehiculo = await _unitOfWork.Repository<Vehiculo>()
            .GetByIdAsync(dto.IdVehiculo, cancellationToken);
        
        if (vehiculo == null)
            throw new NotFoundException("Vehiculo", dto.IdVehiculo);

        // Validar que todas las compras existen y están activas
        var compras = new List<Compra>();
        foreach (var idCompra in dto.IdCompras)
        {
            var compra = await _unitOfWork.Repository<Compra>()
                .GetByIdAsync(idCompra, cancellationToken);
            
            if (compra == null)
                throw new NotFoundException("Compra", idCompra);

            if (compra.Estado != "ACTIVO")
                throw new ValidationException($"La compra con ID {idCompra} no está en estado ACTIVO");

            // Validar que la compra no esté ya asignada a otro vehículo
            var comprasVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>()
                .GetAllAsync(cancellationToken);
            
            var yaAsignada = comprasVehiculoDetalles.Any(cvd => cvd.IdCompra == idCompra);
            if (yaAsignada)
                throw new ValidationException($"La compra con ID {idCompra} ya está asignada a un vehículo");

            compras.Add(compra);
        }

        // Validar que la guía de remisión no esté duplicada
        var comprasVehiculos = await _unitOfWork.Repository<CompraVehiculo>()
            .GetAllAsync(cancellationToken);
        
        if (comprasVehiculos.Any(cv => cv.GuiaRemision.ToUpper() == dto.GuiaRemision.ToUpper()))
            throw new ValidationException($"La guía de remisión '{dto.GuiaRemision}' ya está registrada");

        // Crear el registro de CompraVehiculo
        var compraVehiculo = new CompraVehiculo
        {
            IdPersona = dto.IdPersona,
            IdVehiculo = dto.IdVehiculo,
            GuiaRemision = dto.GuiaRemision.ToUpper().Trim(),
            Estado = "ACTIVO",
            FRegistro = DateTime.Now
        };

        await _unitOfWork.Repository<CompraVehiculo>().AddAsync(compraVehiculo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Crear los detalles (asignar las compras al vehículo)
        var detalles = new List<CompraVehiculoDetalle>();
        foreach (var idCompra in dto.IdCompras)
        {
            var detalle = new CompraVehiculoDetalle
            {
                IdCompraVehiculo = compraVehiculo.IdCompraVehiculo,
                IdCompra = idCompra
            };

            await _unitOfWork.Repository<CompraVehiculoDetalle>().AddAsync(detalle, cancellationToken);
            detalles.Add(detalle);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Actualizar el EstadoRecepcion de las compras a PENDIENTE
        foreach (var compra in compras)
        {
            compra.EstadoRecepcion = "PENDIENTE";
            await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        compraVehiculo.Persona = persona;
        compraVehiculo.Vehiculo = vehiculo;
        compraVehiculo.CompraVehiculoDetalles = detalles;

        // Cargar las compras en los detalles
        foreach (var detalle in detalles)
        {
            detalle.Compra = compras.First(c => c.IdCompra == detalle.IdCompra);
        }

        return _mapper.Map<CompraVehiculoDto>(compraVehiculo);
    }
}
