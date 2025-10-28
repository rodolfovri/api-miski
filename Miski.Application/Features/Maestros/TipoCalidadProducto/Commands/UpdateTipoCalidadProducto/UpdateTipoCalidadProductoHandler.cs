using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.UpdateTipoCalidadProducto;

public class UpdateTipoCalidadProductoHandler : IRequestHandler<UpdateTipoCalidadProductoCommand, TipoCalidadProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTipoCalidadProductoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoCalidadProductoDto> Handle(UpdateTipoCalidadProductoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.TipoCalidadProducto;

        var tipoCalidadProducto = await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (tipoCalidadProducto == null)
            throw new NotFoundException("TipoCalidadProducto", request.Id);

        // Validar que el producto existe
        var producto = await _unitOfWork.Repository<Producto>()
            .GetByIdAsync(dto.IdProducto, cancellationToken);

        if (producto == null)
            throw new NotFoundException("Producto", dto.IdProducto);

        // Actualizar
        tipoCalidadProducto.IdProducto = dto.IdProducto;
        tipoCalidadProducto.Nombre = dto.Nombre;
        tipoCalidadProducto.Estado = dto.Estado ?? tipoCalidadProducto.Estado;

        await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .UpdateAsync(tipoCalidadProducto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relación para el DTO
        tipoCalidadProducto.Producto = producto;

        return _mapper.Map<TipoCalidadProductoDto>(tipoCalidadProducto);
    }
}
