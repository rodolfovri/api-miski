using MediatR;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Auth;
using Miski.Shared.Exceptions;
using Miski.Application.Features.Auth.Commands.Login;

namespace Miski.Application.Features.Auth.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RegisterHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Verificar que la persona existe
        var persona = await _unitOfWork.Repository<Persona>().GetByIdAsync(request.RegisterData.IdPersona, cancellationToken);
        if (persona == null)
        {
            throw new NotFoundException("Persona", request.RegisterData.IdPersona);
        }

        // Verificar que el rol existe
        var rol = await _unitOfWork.Repository<Rol>().GetByIdAsync(request.RegisterData.IdRol, cancellationToken);
        if (rol == null)
        {
            throw new NotFoundException("Rol", request.RegisterData.IdRol);
        }

        // Verificar que la persona no tenga ya un usuario
        var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        var usuarioExistentePorPersona = usuarios.FirstOrDefault(u => u.IdPersona == request.RegisterData.IdPersona);
        if (usuarioExistentePorPersona != null)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "IdPersona", new[] { "Esta persona ya tiene un usuario registrado" } }
            });
        }

        // Verificar que el username no esté en uso
        var usuarioExistentePorUsername = usuarios.FirstOrDefault(u => u.Username.ToLower() == request.RegisterData.Username.ToLower());
        if (usuarioExistentePorUsername != null)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Username", new[] { "Este nombre de usuario ya está en uso" } }
            });
        }

        // Crear hash de la contraseña
        var passwordHash = HashPassword(request.RegisterData.Password);

        // Crear el usuario
        var nuevoUsuario = new Usuario
        {
            IdPersona = request.RegisterData.IdPersona,
            Username = request.RegisterData.Username,
            PasswordHash = passwordHash,
            IdRol = request.RegisterData.IdRol,
            Estado = request.RegisterData.Estado ?? "ACTIVO",
            FRegistro = DateTime.Now
        };

        await _unitOfWork.Repository<Usuario>().AddAsync(nuevoUsuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Hacer login automático después del registro
        var loginCommand = new LoginCommand(new LoginDto
        {
            NumeroDocumento = persona.NumeroDocumento,
            Password = request.RegisterData.Password
        });

        return await _mediator.Send(loginCommand, cancellationToken);
    }

    private static byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}