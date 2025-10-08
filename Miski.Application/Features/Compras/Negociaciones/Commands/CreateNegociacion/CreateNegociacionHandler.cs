using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;

public class CreateNegociacionHandler : IRequestHandler<CreateNegociacionCommand, NegociacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateNegociacionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NegociacionDto> Handle(CreateNegociacionCommand request, CancellationToken cancellationToken)
    {
        // Validar que el proveedor existe si se proporciona
        if (request.Negociacion.IdProveedor.HasValue)
        {
            var proveedor = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(request.Negociacion.IdProveedor.Value, cancellationToken);
            
            if (proveedor == null)
                throw new Shared.Exceptions.NotFoundException("Proveedor no encontrado", request.Negociacion.IdProveedor.Value);
        }

        // Validar que el comisionista existe
        var comisionista = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(request.Negociacion.IdComisionista, cancellationToken);
        
        if (comisionista == null)
            throw new Shared.Exceptions.NotFoundException("Comisionista no encontrado", request.Negociacion.IdComisionista);

        // Validar que el producto existe si se proporciona
        if (request.Negociacion.IdProducto.HasValue)
        {
            var producto = await _unitOfWork.Repository<Producto>()
                .GetByIdAsync(request.Negociacion.IdProducto.Value, cancellationToken);
            
            if (producto == null)
                throw new Shared.Exceptions.NotFoundException("Producto no encontrado", request.Negociacion.IdProducto.Value);
        }

        // Crear la negociación
        var negociacion = new Negociacion
        {
            IdProveedor = request.Negociacion.IdProveedor,
            IdComisionista = request.Negociacion.IdComisionista,
            IdProducto = request.Negociacion.IdProducto,
            PesoTotal = request.Negociacion.PesoTotal,
            PrecioUnitario = request.Negociacion.PrecioUnitario,
            NroCuentaRuc = request.Negociacion.NroCuentaRuc,
            FotoCalidadProducto = request.Negociacion.FotoCalidadProducto,
            FotoDniFrontal = request.Negociacion.FotoDniFrontal,
            FotoDniPosterior = request.Negociacion.FotoDniPosterior,
            Observacion = request.Negociacion.Observacion,
            Estado = "Pendiente"
        };

        await _unitOfWork.Repository<Negociacion>().AddAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NegociacionDto>(negociacion);
    }
}