using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Usuarios;
using Miski.Shared.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System.Collections.ObjectModel;

namespace Miski.Application.Features.Usuarios.Commands.CreateUsuario;

public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioCommand, UsuarioDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUsuarioHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UsuarioDto> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Usuario;

        // Validar que el username no exista
        var usuariosExistentes = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        if (usuariosExistentes.Any(u => u.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase)))
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
            var roles = await _unitOfWork.Repository<Rol>().GetAllAsync(cancellationToken);
            foreach (var rolId in dto.RolesIds)
            {
                if (!roles.Any(r => r.IdRol == rolId))
                {
                    throw new NotFoundException("Rol", rolId);
                }
            }
        }

        // Hash de la contraseña
        var passwordHash = HashPassword(dto.Password);

        // Crear el usuario
        var nuevoUsuario = new Usuario
        {
            IdPersona = dto.IdPersona,
            Username = dto.Username,
            PasswordHash = passwordHash,
            Estado = dto.Estado ?? "ACTIVO",
            FRegistro = DateTime.Now
        };

        await _unitOfWork.Repository<Usuario>().AddAsync(nuevoUsuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Asignar roles
        if (dto.RolesIds != null && dto.RolesIds.Any())
        {
            foreach (var rolId in dto.RolesIds)
            {
                var usuarioRol = new UsuarioRol
                {
                    IdUsuario = nuevoUsuario.IdUsuario,
                    IdRol = rolId
                };
                await _unitOfWork.Repository<UsuarioRol>().AddAsync(usuarioRol, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Cargar relaciones para el DTO
        if (nuevoUsuario.IdPersona.HasValue)
        {
            nuevoUsuario.Persona = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(nuevoUsuario.IdPersona.Value, cancellationToken);
        }

        // Cargar roles
        if (dto.RolesIds != null && dto.RolesIds.Any())
        {
            var rolesData = await _unitOfWork.Repository<Rol>().GetAllAsync(cancellationToken);
            nuevoUsuario.UsuarioRoles = new Collection<UsuarioRol>(dto.RolesIds
                .Select(idRol => new UsuarioRol 
                { 
                    IdUsuario = nuevoUsuario.IdUsuario, 
                    IdRol = idRol,
                    Rol = rolesData.FirstOrDefault(r => r.IdRol == idRol)
                })
                .ToList());
        }

        return _mapper.Map<UsuarioDto>(nuevoUsuario);
    }

    private byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
