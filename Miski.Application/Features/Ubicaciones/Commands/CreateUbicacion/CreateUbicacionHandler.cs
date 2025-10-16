using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Ubicaciones;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Ubicaciones.Commands.CreateUbicacion;

public class CreateUbicacionHandler : IRequestHandler<CreateUbicacionCommand, UbicacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUbicacionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UbicacionDto> Handle(CreateUbicacionCommand request, CancellationToken cancellationToken)
    {
        // Verificar que el usuario existe
        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(request.Ubicacion.IdUsuario, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", request.Ubicacion.IdUsuario);

        // Crear la nueva ubicación
        var nuevaUbicacion = new Ubicacion
        {
            IdUsuario = request.Ubicacion.IdUsuario,
            Nombre = request.Ubicacion.Nombre,
            Direccion = request.Ubicacion.Direccion,
            Tipo = request.Ubicacion.Tipo,
            Estado = request.Ubicacion.Estado,
            FRegistro = DateTime.Now
        };

        await _unitOfWork.Repository<Ubicacion>().AddAsync(nuevaUbicacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar el usuario para el DTO
        nuevaUbicacion.Usuario = usuario;

        return _mapper.Map<UbicacionDto>(nuevaUbicacion);
    }
}