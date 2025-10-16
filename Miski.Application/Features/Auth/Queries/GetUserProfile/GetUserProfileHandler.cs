using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Auth;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Auth.Queries.GetUserProfile;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserProfileHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var usuarioCompleto = await GetUsuarioCompletoAsync(request.UserId, cancellationToken);

        // Construir roles con permisos
        var rolesConPermisos = await BuildRolesConPermisosAsync(usuarioCompleto.UsuarioRoles, cancellationToken);

        return new AuthResponseDto
        {
            IdUsuario = usuarioCompleto.IdUsuario,
            Username = usuarioCompleto.Username,
            Token = string.Empty, // No devolvemos token en el perfil
            Expiration = DateTime.MinValue,
            Persona = usuarioCompleto.Persona != null ? _mapper.Map<AuthPersonaDto>(usuarioCompleto.Persona) : null,
            Roles = rolesConPermisos
        };
    }

    private async Task<Usuario> GetUsuarioCompletoAsync(int usuarioId, CancellationToken cancellationToken)
    {
        var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var usuarioRoles = await _unitOfWork.Repository<UsuarioRol>().GetAllAsync(cancellationToken);
        var roles = await _unitOfWork.Repository<Rol>().GetAllAsync(cancellationToken);
        var tiposDocumento = await _unitOfWork.Repository<TipoDocumento>().GetAllAsync(cancellationToken);

        var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);
        if (usuario == null) 
            throw new NotFoundException("Usuario", usuarioId);

        // Cargar relaciones manualmente
        if (usuario.IdPersona.HasValue)
        {
            usuario.Persona = personas.FirstOrDefault(p => p.IdPersona == usuario.IdPersona.Value);
            if (usuario.Persona != null)
            {
                usuario.Persona.TipoDocumento = tiposDocumento.FirstOrDefault(td => td.IdTipoDocumento == usuario.Persona.IdTipoDocumento);
            }
        }

        // Limpiar la colección existente para evitar duplicados
        usuario.UsuarioRoles.Clear();

        // Cargar roles del usuario a través de UsuarioRol
        var rolesUsuario = usuarioRoles.Where(ur => ur.IdUsuario == usuarioId).ToList();
        foreach (var ur in rolesUsuario)
        {
            ur.Rol = roles.FirstOrDefault(r => r.IdRol == ur.IdRol) ?? new Rol();
            usuario.UsuarioRoles.Add(ur);
        }

        return usuario;
    }

    private async Task<List<RolDto>> BuildRolesConPermisosAsync(
        ICollection<UsuarioRol> usuarioRoles, 
        CancellationToken cancellationToken)
    {
        var resultado = new List<RolDto>();

        // Obtener todas las entidades necesarias para permisos
        var permisos = await _unitOfWork.Repository<PermisoRol>().GetAllAsync(cancellationToken);
        var modulos = await _unitOfWork.Repository<Modulo>().GetAllAsync(cancellationToken);
        var subModulos = await _unitOfWork.Repository<SubModulo>().GetAllAsync(cancellationToken);
        var subModuloDetalles = await _unitOfWork.Repository<SubModuloDetalle>().GetAllAsync(cancellationToken);

        foreach (var ur in usuarioRoles.Distinct())
        {
            var rolDto = new RolDto
            {
                IdRol = ur.Rol.IdRol,
                Nombre = ur.Rol.Nombre,
                Descripcion = ur.Rol.Descripcion,
                TipoPlataforma = ur.Rol.TipoPlataforma,
                Permisos = new List<RolPermisoDto>()
            };

            // Obtener permisos del rol
            var permisosRol = permisos.Where(p => p.IdRol == ur.Rol.IdRol && p.TieneAcceso).ToList();

            foreach (var permiso in permisosRol)
            {
                var permisoDto = new RolPermisoDto
                {
                    IdModulo = permiso.IdModulo,
                    ModuloNombre = permiso.IdModulo.HasValue 
                        ? modulos.FirstOrDefault(m => m.IdModulo == permiso.IdModulo.Value)?.Nombre 
                        : null,
                    IdSubModulo = permiso.IdSubModulo,
                    SubModuloNombre = permiso.IdSubModulo.HasValue 
                        ? subModulos.FirstOrDefault(sm => sm.IdSubModulo == permiso.IdSubModulo.Value)?.Nombre 
                        : null,
                    IdSubModuloDetalle = permiso.IdSubModuloDetalle,
                    SubModuloDetalleNombre = permiso.IdSubModuloDetalle.HasValue 
                        ? subModuloDetalles.FirstOrDefault(smd => smd.IdSubModuloDetalle == permiso.IdSubModuloDetalle.Value)?.Nombre 
                        : null,
                    TieneAcceso = permiso.TieneAcceso
                };

                rolDto.Permisos.Add(permisoDto);
            }

            resultado.Add(rolDto);
        }

        return resultado.Distinct().ToList();
    }
}