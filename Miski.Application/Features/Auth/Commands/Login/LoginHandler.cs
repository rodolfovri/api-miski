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

            // Obtener permisos del rol con acceso habilitado
            var permisosRol = permisos.Where(p => p.IdRol == ur.Rol.IdRol && p.TieneAcceso).ToList();

            // Expandir permisos según la herencia
            var permisosExpandidos = ExpandirPermisos(
                permisosRol,
                modulos,
                subModulos,
                subModuloDetalles,
                tipoPlataforma,
                acciones,
                permisoRolAcciones,
                subModuloAcciones,
                subModuloDetalleAcciones);

            rolDto.Permisos = permisosExpandidos;
            resultado.Add(rolDto);
        }

        return resultado.Distinct().ToList();
    }

    /// <summary>
    /// Expande los permisos según la lógica de herencia
    /// - Módulo: Expande todos sus submódulos y detalles
    /// - SubMódulo: Expande todos sus detalles si TieneDetalles=true
    /// - Detalle: Se mantiene como está
    /// </summary>
    private List<RolPermisoDto> ExpandirPermisos(
        List<PermisoRol> permisosRol,
        IEnumerable<Modulo> modulos,
        IEnumerable<SubModulo> subModulos,
        IEnumerable<SubModuloDetalle> subModuloDetalles,
        string tipoPlataforma,
        IEnumerable<Accion> acciones,
        IEnumerable<PermisoRolAccion> permisoRolAcciones,
        IEnumerable<SubModuloAccion> subModuloAcciones,
        IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        var permisosExpandidos = new List<RolPermisoDto>();

        foreach (var permiso in permisosRol)
        {
            // Validar que el módulo existe y es de la plataforma correcta
            if (!permiso.IdModulo.HasValue)
                continue;

            var modulo = modulos.FirstOrDefault(m => m.IdModulo == permiso.IdModulo.Value);
            if (modulo == null || modulo.TipoPlataforma != tipoPlataforma)
                continue;

            // CASO 1: Permiso a nivel de MÓDULO (solo IdModulo)
            if (!permiso.IdSubModulo.HasValue && !permiso.IdSubModuloDetalle.HasValue)
            {
                var permisosModulo = ExpandirPermisoModulo(
                    permiso,
                    modulo,
                    subModulos,
                    subModuloDetalles,
                    acciones,
                    permisoRolAcciones,
                    subModuloAcciones,
                    subModuloDetalleAcciones);

                permisosExpandidos.AddRange(permisosModulo);
            }
            // CASO 2: Permiso a nivel de SUBMÓDULO (IdModulo + IdSubModulo)
            else if (permiso.IdSubModulo.HasValue && !permiso.IdSubModuloDetalle.HasValue)
            {
                var subModulo = subModulos.FirstOrDefault(sm => sm.IdSubModulo == permiso.IdSubModulo.Value);
                if (subModulo != null && subModulo.Estado == "ACTIVO")
                {
                    var permisosSubModulo = ExpandirPermisoSubModulo(
                        permiso,
                        modulo,
                        subModulo,
                        subModuloDetalles,
                        acciones,
                        permisoRolAcciones,
                        subModuloAcciones,
                        subModuloDetalleAcciones);

                    permisosExpandidos.AddRange(permisosSubModulo);
                }
            }
            // CASO 3: Permiso a nivel de DETALLE (IdModulo + IdSubModulo + IdSubModuloDetalle)
            else if (permiso.IdSubModuloDetalle.HasValue)
            {
                var subModulo = subModulos.FirstOrDefault(sm => sm.IdSubModulo == permiso.IdSubModulo.Value);
                var detalle = subModuloDetalles.FirstOrDefault(smd => smd.IdSubModuloDetalle == permiso.IdSubModuloDetalle.Value);

                if (subModulo != null && detalle != null && detalle.Estado == "ACTIVO")
                {
                    var permisoDetalle = CrearPermisoDetalle(
                        permiso,
                        modulo,
                        subModulo,
                        detalle,
                        acciones,
                        permisoRolAcciones,
                        subModuloDetalleAcciones);

                    permisosExpandidos.Add(permisoDetalle);
                }
            }
        }

        // Eliminar duplicados usando un HashSet con una clave única
        var permisosUnicos = permisosExpandidos
            .GroupBy(p => $"{p.IdModulo}_{p.IdSubModulo}_{p.IdSubModuloDetalle}")
            .Select(g => g.First())
            .ToList();

        return permisosUnicos;
    }

    /// <summary>
    /// Expande un permiso a nivel de MÓDULO completo
    /// Incluye TODOS los submódulos y sus detalles (si aplica)
    /// </summary>
    private List<RolPermisoDto> ExpandirPermisoModulo(
        PermisoRol permiso,
        Modulo modulo,
        IEnumerable<SubModulo> subModulos,
        IEnumerable<SubModuloDetalle> subModuloDetalles,
        IEnumerable<Accion> acciones,
        IEnumerable<PermisoRolAccion> permisoRolAcciones,
        IEnumerable<SubModuloAccion> subModuloAcciones,
        IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        var permisos = new List<RolPermisoDto>();

        // Obtener todos los submódulos activos del módulo
        var subModulosDelModulo = subModulos
            .Where(sm => sm.IdModulo == modulo.IdModulo && sm.Estado == "ACTIVO")
            .OrderBy(sm => sm.Orden)
            .ToList();

        foreach (var subModulo in subModulosDelModulo)
        {
            // Si el submódulo tiene detalles, expandir cada detalle
            if (subModulo.TieneDetalles)
            {
                var detallesDelSubModulo = subModuloDetalles
                    .Where(d => d.IdSubModulo == subModulo.IdSubModulo && d.Estado == "ACTIVO")
                    .OrderBy(d => d.Orden)
                    .ToList();

                foreach (var detalle in detallesDelSubModulo)
                {
                    var permisoDetalle = CrearPermisoDetalle(
                        permiso,
                        modulo,
                        subModulo,
                        detalle,
                        acciones,
                        permisoRolAcciones,
                        subModuloDetalleAcciones);

                    permisos.Add(permisoDetalle);
                }
            }
            else
            {
                // Submódulo sin detalles - incluir directamente
                var permisoSubModulo = CrearPermisoSubModulo(
                    permiso,
                    modulo,
                    subModulo,
                    acciones,
                    permisoRolAcciones,
                    subModuloAcciones);

                permisos.Add(permisoSubModulo);
            }
        }

        return permisos;
    }

    /// <summary>
    /// Expande un permiso a nivel de SUBMÓDULO específico
    /// Si TieneDetalles=true, expande TODOS sus detalles
    /// Si TieneDetalles=false, retorna el submódulo directamente
    /// </summary>
    private List<RolPermisoDto> ExpandirPermisoSubModulo(
        PermisoRol permiso,
        Modulo modulo,
        SubModulo subModulo,
        IEnumerable<SubModuloDetalle> subModuloDetalles,
        IEnumerable<Accion> acciones,
        IEnumerable<PermisoRolAccion> permisoRolAcciones,
        IEnumerable<SubModuloAccion> subModuloAcciones,
        IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        var permisos = new List<RolPermisoDto>();

        if (subModulo.TieneDetalles)
        {
            // Expandir todos los detalles del submódulo
            var detallesDelSubModulo = subModuloDetalles
                .Where(d => d.IdSubModulo == subModulo.IdSubModulo && d.Estado == "ACTIVO")
                .OrderBy(d => d.Orden)
                .ToList();

            foreach (var detalle in detallesDelSubModulo)
            {
                var permisoDetalle = CrearPermisoDetalle(
                    permiso,
                    modulo,
                    subModulo,
                    detalle,
                    acciones,
                    permisoRolAcciones,
                    subModuloDetalleAcciones);

                permisos.Add(permisoDetalle);
            }
        }
        else
        {
            // Submódulo sin detalles - incluir directamente
            var permisoSubModulo = CrearPermisoSubModulo(
                permiso,
                modulo,
                subModulo,
                acciones,
                permisoRolAcciones,
                subModuloAcciones);

            permisos.Add(permisoSubModulo);
        }

        return permisos;
    }

    /// <summary>
    /// Crea un DTO de permiso para un SubMódulo sin detalles
    /// </summary>
    private RolPermisoDto CrearPermisoSubModulo(
    PermisoRol permiso,
    Modulo modulo,
    SubModulo subModulo,
    IEnumerable<Accion> acciones,
    IEnumerable<PermisoRolAccion> permisoRolAcciones,
    IEnumerable<SubModuloAccion> subModuloAcciones)
    {
        return new RolPermisoDto
        {
            IdModulo = modulo.IdModulo,
            ModuloNombre = modulo.Nombre,
            ModuloRuta = modulo.Ruta,
            ModuloIcono = modulo.Icono,
            IdSubModulo = subModulo.IdSubModulo,
            SubModuloNombre = subModulo.Nombre,
            SubModuloRuta = subModulo.Ruta,
            SubModuloIcono = subModulo.Icono,
            SubModuloTieneDetalles = subModulo.TieneDetalles,
            IdSubModuloDetalle = null,
            SubModuloDetalleNombre = null,
            SubModuloDetalleRuta = null,
            SubModuloDetalleIcono = null,
            TieneAcceso = true,
            Acciones = BuildAccionesParaPermiso(
                permiso,
                subModulo.IdSubModulo,  // 👈 NUEVO: Pasar el ID del submódulo actual
                null,                   // 👈 NUEVO: No hay detalle
                subModulo,
                acciones,
                permisoRolAcciones,
                subModuloAcciones,
                new List<SubModuloDetalleAccion>())
        };
    }

    /// <summary>
    /// Crea un DTO de permiso para un SubMóduloDetalle específico
    /// </summary>
    private RolPermisoDto CrearPermisoDetalle(
     PermisoRol permiso,
     Modulo modulo,
     SubModulo subModulo,
     SubModuloDetalle detalle,
     IEnumerable<Accion> acciones,
     IEnumerable<PermisoRolAccion> permisoRolAcciones,
     IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        return new RolPermisoDto
        {
            IdModulo = modulo.IdModulo,
            ModuloNombre = modulo.Nombre,
            ModuloRuta = modulo.Ruta,
            ModuloIcono = modulo.Icono,
            IdSubModulo = subModulo.IdSubModulo,
            SubModuloNombre = subModulo.Nombre,
            SubModuloRuta = subModulo.Ruta,
            SubModuloIcono = subModulo.Icono,
            SubModuloTieneDetalles = subModulo.TieneDetalles,
            IdSubModuloDetalle = detalle.IdSubModuloDetalle,
            SubModuloDetalleNombre = detalle.Nombre,
            SubModuloDetalleRuta = detalle.Ruta,
            SubModuloDetalleIcono = detalle.Icono,
            TieneAcceso = true,
            Acciones = BuildAccionesParaPermiso(
                permiso,
                subModulo.IdSubModulo,        // 👈 NUEVO: Pasar el ID del submódulo actual
                detalle.IdSubModuloDetalle,   // 👈 NUEVO: Pasar el ID del detalle actual
                subModulo,
                acciones,
                permisoRolAcciones,
                new List<SubModuloAccion>(),
                subModuloDetalleAcciones)
        };
    }

    /// <summary>
    /// Construye la lista de acciones disponibles para un permiso con su estado de habilitación
    /// LÓGICA:
    /// - Herencia (permiso expandido): Todas las acciones disponibles están habilitadas
    /// - Permiso específico: Solo las marcadas en PermisoRolAccion están habilitadas
    /// </summary>
    private List<RolAccionDto> BuildAccionesParaPermiso(
        PermisoRol permisoOriginal,
        int? idSubModuloActual,        // 👈 NUEVO: El submódulo que estamos procesando
        int? idSubModuloDetalleActual, // 👈 NUEVO: El detalle que estamos procesando
        SubModulo? subModulo,
        IEnumerable<Accion> acciones,
        IEnumerable<PermisoRolAccion> permisoRolAcciones,
        IEnumerable<SubModuloAccion> subModuloAcciones,
        IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        var resultado = new List<RolAccionDto>();

        // Determinar si es un permiso por HERENCIA o ESPECÍFICO
        bool esPermisoHerencia = DeterminarSiEsHerencia(permisoOriginal, subModulo);

        HashSet<int> accionesHabilitadas;
        HashSet<int> accionesDisponibles;

        // Caso 1: Permiso a nivel de SubMóduloDetalle
        if (idSubModuloDetalleActual.HasValue) // 👈 CAMBIO: Usar el detalle actual
        {
            // Obtener acciones DISPONIBLES para este detalle
            accionesDisponibles = subModuloDetalleAcciones
                .Where(smda => smda.IdSubModuloDetalle == idSubModuloDetalleActual.Value) // 👈 CAMBIO
                .Select(smda => smda.IdAccion)
                .ToHashSet();

            // Determinar cuáles están HABILITADAS
            if (esPermisoHerencia)
            {
                // Herencia: Todas las disponibles están habilitadas
                accionesHabilitadas = accionesDisponibles;
            }
            else
            {
                // Específico: Solo las marcadas en PermisoRolAccion
                accionesHabilitadas = permisoRolAcciones
                    .Where(pra => pra.IdPermisoRol == permisoOriginal.IdPermisoRol && pra.Habilitado)
                    .Select(pra => pra.IdAccion)
                    .ToHashSet();
            }
        }
        // Caso 2: Permiso a nivel de SubMódulo (sin detalles)
        else if (idSubModuloActual.HasValue && subModulo != null && !subModulo.TieneDetalles) // 👈 CAMBIO
        {
            // Obtener acciones DISPONIBLES para este submódulo
            accionesDisponibles = subModuloAcciones
                .Where(sma => sma.IdSubModulo == idSubModuloActual.Value) // 👈 CAMBIO
                .Select(sma => sma.IdAccion)
                .ToHashSet();

            // Determinar cuáles están HABILITADAS
            if (esPermisoHerencia)
            {
                // Herencia: Todas las disponibles están habilitadas
                accionesHabilitadas = accionesDisponibles;
            }
            else
            {
                // Específico: Solo las marcadas en PermisoRolAccion
                accionesHabilitadas = permisoRolAcciones
                    .Where(pra => pra.IdPermisoRol == permisoOriginal.IdPermisoRol && pra.Habilitado)
                    .Select(pra => pra.IdAccion)
                    .ToHashSet();
            }
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

    /// <summary>
    /// Determina si un permiso fue generado por HERENCIA o es ESPECÍFICO
    /// </summary>
    private bool DeterminarSiEsHerencia(PermisoRol permisoOriginal, SubModulo? subModulo)
    {
        // Es herencia si:
        // 1. El permiso original es a nivel de MÓDULO (no tiene IdSubModulo)
        // 2. El permiso original es a nivel de SUBMÓDULO completo (no tiene IdSubModuloDetalle)
        //    Y ese submódulo tiene detalles (TieneDetalles=true)

        if (!permisoOriginal.IdSubModulo.HasValue)
        {
            // Permiso de módulo completo = HERENCIA
            return true;
        }

        if (!permisoOriginal.IdSubModuloDetalle.HasValue && subModulo != null && subModulo.TieneDetalles)
        {
            // Permiso de submódulo con detalles = HERENCIA
            return true;
        }

        // En cualquier otro caso, es un permiso ESPECÍFICO
        return false;
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