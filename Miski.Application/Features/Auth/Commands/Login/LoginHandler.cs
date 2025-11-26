using MediatR;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Auth;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public LoginHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Buscar usuario por número de documento de la persona relacionada
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var persona = personas.FirstOrDefault(p => p.NumeroDocumento == request.LoginData.NumeroDocumento);

        if (persona == null)
        {
            throw new NotFoundException("Usuario", request.LoginData.NumeroDocumento);
        }

        // Buscar el usuario asociado a esa persona
        var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        var usuario = usuarios.FirstOrDefault(u =>
            u.IdPersona == persona.IdPersona &&
            u.Estado == "ACTIVO");

        if (usuario == null)
        {
            throw new NotFoundException("Usuario", request.LoginData.NumeroDocumento);
        }

        // Verificar contraseña
        if (!VerifyPassword(request.LoginData.Password, usuario.PasswordHash))
        {
            throw new ValidationException("Contraseña incorrecta");
        }

        // Registrar dispositivo si es Mobile
        if (request.LoginData.TipoPlataforma.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
        {
            await RegisterOrUpdateDispositivoAsync(persona.IdPersona, request.LoginData, cancellationToken);
        }

        // Cargar datos relacionados incluyendo roles y permisos
        var usuarioCompleto = await GetUsuarioCompletoAsync(usuario.IdUsuario, request.LoginData.TipoPlataforma, cancellationToken);

        // Generar token JWT
        var token = GenerateJwtToken(usuarioCompleto);

        // Construir la respuesta con roles y sus permisos
        var rolesConPermisos = await BuildRolesConPermisosAsync(usuarioCompleto.UsuarioRoles, request.LoginData.TipoPlataforma, cancellationToken);

        return new AuthResponseDto
        {
            IdUsuario = usuarioCompleto.IdUsuario,
            Username = usuarioCompleto.Username,
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(8),
            Persona = usuarioCompleto.Persona != null ? _mapper.Map<AuthPersonaDto>(usuarioCompleto.Persona) : null,
            Roles = rolesConPermisos
        };
    }

    private async Task RegisterOrUpdateDispositivoAsync(int idPersona, LoginDto loginData, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(loginData.DeviceId))
            return;

        var dispositivos = await _unitOfWork.Repository<DispositivoPersona>().GetAllAsync(cancellationToken);
        var dispositivo = dispositivos.FirstOrDefault(d => d.DeviceId == loginData.DeviceId);

        if (dispositivo == null)
        {
            // Primera vez que se registra este dispositivo
            dispositivo = new DispositivoPersona
            {
                IdPersona = idPersona,
                DeviceId = loginData.DeviceId,
                ModeloDispositivo = loginData.ModeloDispositivo,
                SistemaOperativo = loginData.SistemaOperativo,
                VersionApp = loginData.VersionApp,
                FRegistro = DateTime.UtcNow,
                FUltimaActividad = DateTime.UtcNow,
                Activo = true
            };
            await _unitOfWork.Repository<DispositivoPersona>().AddAsync(dispositivo);
        }
        else
        {
            // Dispositivo conocido, actualizar actividad y versión
            dispositivo.FUltimaActividad = DateTime.UtcNow;
            dispositivo.VersionApp = loginData.VersionApp ?? dispositivo.VersionApp;
            dispositivo.Activo = true;
            await _unitOfWork.Repository<DispositivoPersona>().UpdateAsync(dispositivo);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Usuario> GetUsuarioCompletoAsync(int usuarioId, string tipoPlataforma, CancellationToken cancellationToken)
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

        // Cargar roles del usuario a través de UsuarioRol, filtrando por TipoPlataforma
        var rolesUsuario = usuarioRoles.Where(ur => ur.IdUsuario == usuarioId).ToList();
        foreach (var ur in rolesUsuario)
        {
            var rol = roles.FirstOrDefault(r => r.IdRol == ur.IdRol);
            if (rol != null)
            {
                // Filtrar por tipo de plataforma
                if (string.IsNullOrEmpty(rol.TipoPlataforma) || rol.TipoPlataforma == tipoPlataforma)
                {
                    ur.Rol = rol;
                    usuario.UsuarioRoles.Add(ur);
                }
            }
        }

        return usuario;
    }

    private async Task<List<RolDto>> BuildRolesConPermisosAsync(
        ICollection<UsuarioRol> usuarioRoles, 
        string tipoPlataforma, 
        CancellationToken cancellationToken)
    {
        var resultado = new List<RolDto>();

        // Obtener todas las entidades necesarias para permisos
        var permisos = await _unitOfWork.Repository<PermisoRol>().GetAllAsync(cancellationToken);
        var modulos = await _unitOfWork.Repository<Modulo>().GetAllAsync(cancellationToken);
        var subModulos = await _unitOfWork.Repository<SubModulo>().GetAllAsync(cancellationToken);
        var subModuloDetalles = await _unitOfWork.Repository<SubModuloDetalle>().GetAllAsync(cancellationToken);
        
        // Obtener datos de acciones
        var acciones = await _unitOfWork.Repository<Accion>().GetAllAsync(cancellationToken);
        var permisoRolAcciones = await _unitOfWork.Repository<PermisoRolAccion>().GetAllAsync(cancellationToken);
        var subModuloAcciones = await _unitOfWork.Repository<SubModuloAccion>().GetAllAsync(cancellationToken);
        var subModuloDetalleAcciones = await _unitOfWork.Repository<SubModuloDetalleAccion>().GetAllAsync(cancellationToken);

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

            // Obtener permisos del rol filtrando por módulos de la plataforma
            var permisosRol = permisos.Where(p => p.IdRol == ur.Rol.IdRol && p.TieneAcceso).ToList();

            foreach (var permiso in permisosRol)
            {
                // Verificar que el módulo corresponda a la plataforma
                if (permiso.IdModulo.HasValue)
                {
                    var modulo = modulos.FirstOrDefault(m => m.IdModulo == permiso.IdModulo.Value);
                    if (modulo != null && modulo.TipoPlataforma == tipoPlataforma)
                    {
                        var subModulo = permiso.IdSubModulo.HasValue 
                            ? subModulos.FirstOrDefault(sm => sm.IdSubModulo == permiso.IdSubModulo.Value)
                            : null;

                        var subModuloDetalle = permiso.IdSubModuloDetalle.HasValue 
                            ? subModuloDetalles.FirstOrDefault(smd => smd.IdSubModuloDetalle == permiso.IdSubModuloDetalle.Value)
                            : null;

                        var permisoDto = new RolPermisoDto
                        {
                            IdModulo = permiso.IdModulo,
                            ModuloNombre = modulo.Nombre,
                            ModuloRuta = modulo.Ruta,
                            ModuloIcono = modulo.Icono,
                            IdSubModulo = permiso.IdSubModulo,
                            SubModuloNombre = subModulo?.Nombre,
                            SubModuloRuta = subModulo?.Ruta,
                            SubModuloIcono = subModulo?.Icono,
                            SubModuloTieneDetalles = subModulo?.TieneDetalles,
                            IdSubModuloDetalle = permiso.IdSubModuloDetalle,
                            SubModuloDetalleNombre = subModuloDetalle?.Nombre,
                            SubModuloDetalleRuta = subModuloDetalle?.Ruta,
                            SubModuloDetalleIcono = subModuloDetalle?.Icono,
                            TieneAcceso = permiso.TieneAcceso,
                            Acciones = BuildAccionesParaPermiso(
                                permiso,
                                subModulo,
                                acciones,
                                permisoRolAcciones,
                                subModuloAcciones,
                                subModuloDetalleAcciones)
                        };

                        rolDto.Permisos.Add(permisoDto);
                    }
                }
            }

            resultado.Add(rolDto);
        }

        return resultado.Distinct().ToList();
    }

    /// <summary>
    /// Construye la lista de acciones disponibles para un permiso con su estado de habilitación
    /// </summary>
    private List<RolAccionDto> BuildAccionesParaPermiso(
        PermisoRol permiso,
        SubModulo? subModulo,
        IEnumerable<Accion> acciones,
        IEnumerable<PermisoRolAccion> permisoRolAcciones,
        IEnumerable<SubModuloAccion> subModuloAcciones,
        IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        var resultado = new List<RolAccionDto>();

        // Obtener las acciones habilitadas para este permiso
        var accionesHabilitadas = permisoRolAcciones
            .Where(pra => pra.IdPermisoRol == permiso.IdPermisoRol && pra.Habilitado)
            .Select(pra => pra.IdAccion)
            .ToHashSet();

        HashSet<int> accionesDisponibles;

        if (permiso.IdSubModuloDetalle.HasValue)
        {
            // Obtener acciones disponibles para SubMóduloDetalle
            accionesDisponibles = subModuloDetalleAcciones
                .Where(smda => smda.IdSubModuloDetalle == permiso.IdSubModuloDetalle.Value && smda.Habilitado)
                .Select(smda => smda.IdAccion)
                .ToHashSet();
        }
        else if (permiso.IdSubModulo.HasValue && subModulo != null && !subModulo.TieneDetalles)
        {
            // Obtener acciones disponibles para SubMódulo (solo si no tiene detalles)
            accionesDisponibles = subModuloAcciones
                .Where(sma => sma.IdSubModulo == permiso.IdSubModulo.Value && sma.Habilitado)
                .Select(sma => sma.IdAccion)
                .ToHashSet();
        }
        else
        {
            // Nivel de Módulo o SubMódulo con detalles - no tiene acciones directas
            return resultado;
        }

        // Construir la lista de acciones con su estado
        resultado = acciones
            .Where(a => a.Estado == "ACTIVO" && accionesDisponibles.Contains(a.IdAccion))
            .OrderBy(a => a.Orden)
            .Select(a => new RolAccionDto
            {
                IdAccion = a.IdAccion,
                Nombre = a.Nombre,
                Codigo = a.Codigo,
                Icono = a.Icono,
                Orden = a.Orden,
                Habilitado = accionesHabilitadas.Contains(a.IdAccion)
            })
            .ToList();

        return resultado;
    }

    private static bool VerifyPassword(string password, byte[] storedHash)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return hash.SequenceEqual(storedHash);
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "MiskiSecretKey2024!@#$%";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "MiskiApi";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "MiskiClient";

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Name, usuario.Username)
        };

        // Agregar múltiples roles como claims (sin duplicados)
        var rolesUnicos = usuario.UsuarioRoles
            .Select(ur => ur.Rol.Nombre)
            .Distinct()
            .ToList();

        foreach (var rolNombre in rolesUnicos)
        {
            claims.Add(new Claim(ClaimTypes.Role, rolNombre));
        }

        if (usuario.Persona != null)
        {
            claims.Add(new Claim("PersonaId", usuario.Persona.IdPersona.ToString()));
            claims.Add(new Claim("NombreCompleto", $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}"));
            claims.Add(new Claim("NumeroDocumento", usuario.Persona.NumeroDocumento));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}