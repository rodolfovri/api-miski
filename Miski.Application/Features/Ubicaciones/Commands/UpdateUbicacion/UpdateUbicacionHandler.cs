using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Ubicaciones;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Ubicaciones.Commands.UpdateUbicacion;

public class UpdateUbicacionHandler : IRequestHandler<UpdateUbicacionCommand, UbicacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUbicacionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UbicacionDto> Handle(UpdateUbicacionCommand request, CancellationToken cancellationToken)
    {
        // Buscar la ubicación
        var ubicacion = await _unitOfWork.Repository<Ubicacion>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (ubicacion == null)
            throw new NotFoundException("Ubicacion", request.Id);

        // Verificar que el usuario existe
        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(request.Ubicacion.IdUsuario, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", request.Ubicacion.IdUsuario);

        // Actualizar la ubicación
        ubicacion.IdUsuario = request.Ubicacion.IdUsuario;
        ubicacion.Nombre = request.Ubicacion.Nombre;
        ubicacion.Direccion = request.Ubicacion.Direccion;
        ubicacion.Tipo = request.Ubicacion.Tipo;
        
        if (!string.IsNullOrEmpty(request.Ubicacion.Estado))
        {
            ubicacion.Estado = request.Ubicacion.Estado;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar el usuario para el DTO
        ubicacion.Usuario = usuario;

        return _mapper.Map<UbicacionDto>(ubicacion);
    }
}