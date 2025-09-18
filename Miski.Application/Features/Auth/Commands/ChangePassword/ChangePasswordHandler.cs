using MediatR;
using System.Security.Cryptography;
using System.Text;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Buscar el usuario
        var usuario = await _unitOfWork.Repository<Usuario>().GetByIdAsync(request.UserId, cancellationToken);
        if (usuario == null)
        {
            throw new NotFoundException("Usuario", request.UserId);
        }

        // Verificar contrase�a actual
        if (!VerifyPassword(request.ChangePasswordData.CurrentPassword, usuario.PasswordHash))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "CurrentPassword", new[] { "La contrase�a actual es incorrecta" } }
            });
        }

        // Generar hash de la nueva contrase�a
        var newPasswordHash = HashPassword(request.ChangePasswordData.NewPassword);

        // Actualizar la contrase�a
        usuario.PasswordHash = newPasswordHash;
        
        await _unitOfWork.Repository<Usuario>().UpdateAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static bool VerifyPassword(string password, byte[] storedHash)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return hash.SequenceEqual(storedHash);
    }

    private static byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}