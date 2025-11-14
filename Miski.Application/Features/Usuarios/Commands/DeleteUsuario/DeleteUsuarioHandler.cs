using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Usuarios.Commands.DeleteUsuario;

public class DeleteUsuarioHandler : IRequestHandler<DeleteUsuarioCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUsuarioHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", request.Id);

        // Cambiar estado a INACTIVO en lugar de eliminar físicamente
        usuario.Estado = "INACTIVO";

        await _unitOfWork.Repository<Usuario>().UpdateAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
