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


        // Cargar datos relacionados
        var usuarioCompleto = await GetUsuarioCompletoAsync(usuario.IdUsuario, cancellationToken);

        // Generar token JWT
        var token = GenerateJwtToken(usuarioCompleto);

        return new AuthResponseDto
        {
            IdUsuario = usuarioCompleto.IdUsuario,
            Username = usuarioCompleto.Username,
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(8),
            Persona = usuarioCompleto.Persona != null ? _mapper.Map<PersonaDto>(usuarioCompleto.Persona) : null,
            Rol = _mapper.Map<RolDto>(usuarioCompleto.Rol)
        };
    }

    private async Task<Usuario> GetUsuarioCompletoAsync(int usuarioId, CancellationToken cancellationToken)
    {
        var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
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

        usuario.Rol = roles.FirstOrDefault(r => r.IdRol == usuario.IdRol);

        return usuario;
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
            new(ClaimTypes.Name, usuario.Username),
            new(ClaimTypes.Role, usuario.Rol.Nombre)
        };

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