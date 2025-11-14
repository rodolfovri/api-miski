using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace Miski.Application.Features.Usuarios.Commands.UpdatePassword;

public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePasswordHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Password;

        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(dto.IdUsuario, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", dto.IdUsuario);

        // Hash de la nueva contraseña
        usuario.PasswordHash = HashPassword(dto.NewPassword);

        await _unitOfWork.Repository<Usuario>().UpdateAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private byte[] HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
