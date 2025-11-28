using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Usuarios;
using Miski.Shared.Exceptions;
using System.Collections.ObjectModel;
using CategoriaPersonaEntity = Miski.Domain.Entities.CategoriaPersona;

namespace Miski.Application.Features.Usuarios.Queries.GetUsuariosByCategoria;

public class GetUsuariosByCategoriaHandler : IRequestHandler<GetUsuariosByCategoriaQuery, List<UsuarioDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUsuariosByCategoriaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UsuarioDto>> Handle(GetUsuariosByCategoriaQuery request, CancellationToken cancellationToken)
    {
        // Verificar que la categoría existe
        var categoria = await _unitOfWork.Repository<CategoriaPersonaEntity>()
            .GetByIdAsync(request.IdCategoria, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaPersona", request.IdCategoria);

        // Obtener todas las relaciones PersonaCategoria
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>()
            .GetAllAsync(cancellationToken);

        // Filtrar personas que pertenecen a la categoría
        var personaIdsEnCategoria = personaCategorias
            .Where(pc => pc.IdCategoria == request.IdCategoria)
            .Select(pc => pc.IdPersona)
            .ToList();

        // Obtener todos los usuarios
        var todosUsuarios = await _unitOfWork.Repository<Usuario>()
            .GetAllAsync(cancellationToken);

        // Filtrar usuarios cuyas personas están en la categoría
        var usuarios = todosUsuarios
            .Where(u => u.IdPersona.HasValue && personaIdsEnCategoria.Contains(u.IdPersona.Value))
            .ToList();

        // Aplicar filtro por estado si se proporciona
        if (!string.IsNullOrEmpty(request.Estado))
        {
            usuarios = usuarios
                .Where(u => u.Estado == request.Estado)
                .ToList();
        }

        // Cargar relaciones
        var personas = await _unitOfWork.Repository<Persona>()
            .GetAllAsync(cancellationToken);
        var usuarioRoles = await _unitOfWork.Repository<UsuarioRol>()
            .GetAllAsync(cancellationToken);
        var roles = await _unitOfWork.Repository<Rol>()
            .GetAllAsync(cancellationToken);

        foreach (var usuario in usuarios)
        {
            if (usuario.IdPersona.HasValue)
            {
                usuario.Persona = personas.FirstOrDefault(p => p.IdPersona == usuario.IdPersona.Value);
            }

            // Cargar roles del usuario
            var rolesUsuario = usuarioRoles
                .Where(ur => ur.IdUsuario == usuario.IdUsuario)
                .Select(ur => ur.IdRol)
                .ToList();

            usuario.UsuarioRoles = new Collection<UsuarioRol>(rolesUsuario
                .Select(idRol => new UsuarioRol 
                { 
                    IdUsuario = usuario.IdUsuario, 
                    IdRol = idRol,
                    Rol = roles.FirstOrDefault(r => r.IdRol == idRol)
                })
                .ToList());
        }

        return usuarios.Select(u => _mapper.Map<UsuarioDto>(u)).ToList();
    }
}
