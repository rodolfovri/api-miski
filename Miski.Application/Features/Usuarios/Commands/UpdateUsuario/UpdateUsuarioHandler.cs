using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Usuarios;
using Miski.Shared.Exceptions;
using System.Collections.ObjectModel;

namespace Miski.Application.Features.Usuarios.Commands.UpdateUsuario;

public class UpdateUsuarioHandler : IRequestHandler<UpdateUsuarioCommand, UsuarioDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUsuarioHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UsuarioDto> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Usuario;

        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(dto.IdUsuario, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", dto.IdUsuario);

        // Validar que el username no exista (excepto el mismo usuario)
        var usuariosExistentes = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        if (usuariosExistentes.Any(u => 
            u.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase) && 
            u.IdUsuario != dto.IdUsuario))
        {
            throw new ValidationException("El nombre de usuario ya existe");
        }

        // Validar que la persona existe si se proporciona
        if (dto.IdPersona.HasValue)
        {
            var persona = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(dto.IdPersona.Value, cancellationToken);
            
            if (persona == null)
                throw new NotFoundException("Persona", dto.IdPersona.Value);
        }

        // Validar que los roles existen
        if (dto.RolesIds != null && dto.RolesIds.Any())
        {
            var rolesValidacion = await _unitOfWork.Repository<Rol>().GetAllAsync(cancellationToken);
            foreach (var rolId in dto.RolesIds)
            {
                if (!rolesValidacion.Any(r => r.IdRol == rolId))
                {
                    throw new NotFoundException("Rol", rolId);
                }
            }
        }

        // Actualizar el usuario
        usuario.IdPersona = dto.IdPersona;
        usuario.Username = dto.Username;
        
        if (!string.IsNullOrEmpty(dto.Estado))
        {
            usuario.Estado = dto.Estado;
        }

        await _unitOfWork.Repository<Usuario>().UpdateAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Actualizar roles
        var usuarioRolesExistentes = await _unitOfWork.Repository<UsuarioRol>().GetAllAsync(cancellationToken);
        var rolesActuales = usuarioRolesExistentes.Where(ur => ur.IdUsuario == usuario.IdUsuario).ToList();

        // Eliminar roles que ya no están en la lista
        foreach (var rolActual in rolesActuales)
        {
            if (dto.RolesIds == null || !dto.RolesIds.Contains(rolActual.IdRol))
            {
                await _unitOfWork.Repository<UsuarioRol>().DeleteAsync(rolActual, cancellationToken);
            }
        }

        // Agregar nuevos roles
        if (dto.RolesIds != null)
        {
            foreach (var rolId in dto.RolesIds)
            {
                if (!rolesActuales.Any(ra => ra.IdRol == rolId))
                {
                    var nuevoUsuarioRol = new UsuarioRol
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdRol = rolId
                    };
                    await _unitOfWork.Repository<UsuarioRol>().AddAsync(nuevoUsuarioRol, cancellationToken);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        if (usuario.IdPersona.HasValue)
        {
            usuario.Persona = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(usuario.IdPersona.Value, cancellationToken);
        }

        // Cargar roles actualizados
        var usuarioRolesActualizados = await _unitOfWork.Repository<UsuarioRol>().GetAllAsync(cancellationToken);
        var rolesData = await _unitOfWork.Repository<Rol>().GetAllAsync(cancellationToken);

        var rolesUsuario = usuarioRolesActualizados
            .Where(ur => ur.IdUsuario == usuario.IdUsuario)
            .Select(ur => ur.IdRol)
            .ToList();

        usuario.UsuarioRoles = new Collection<UsuarioRol>(rolesUsuario
            .Select(idRol => new UsuarioRol 
            { 
                IdUsuario = usuario.IdUsuario, 
                IdRol = idRol,
                Rol = rolesData.FirstOrDefault(r => r.IdRol == idRol)
            })
            .ToList());

        return _mapper.Map<UsuarioDto>(usuario);
    }
}
