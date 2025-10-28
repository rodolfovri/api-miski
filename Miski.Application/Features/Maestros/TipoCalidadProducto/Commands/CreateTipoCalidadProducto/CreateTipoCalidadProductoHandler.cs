using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.CreateTipoCalidadProducto;

public class CreateTipoCalidadProductoHandler : IRequestHandler<CreateTipoCalidadProductoCommand, TipoCalidadProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTipoCalidadProductoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoCalidadProductoDto> Handle(CreateTipoCalidadProductoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.TipoCalidadProducto;

        // Validar que el producto existe
        var producto = await _unitOfWork.Repository<Producto>()
            .GetByIdAsync(dto.IdProducto, cancellationToken);

        if (producto == null)
            throw new NotFoundException("Producto", dto.IdProducto);

        var tipoCalidadProducto = new Domain.Entities.TipoCalidadProducto
        {
            IdProducto = dto.IdProducto,
            Nombre = dto.Nombre,
            Estado = dto.Estado ?? "ACTIVO"  // Usa el estado proporcionado o "ACTIVO" por defecto
        };

        await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .AddAsync(tipoCalidadProducto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relación para el DTO
        tipoCalidadProducto.Producto = producto;

        return _mapper.Map<TipoCalidadProductoDto>(tipoCalidadProducto);
    }
}
