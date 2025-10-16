using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.Commands.DeletePersona;

public class DeletePersonaHandler : IRequestHandler<DeletePersonaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeletePersonaCommand request, CancellationToken cancellationToken)
    {
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (persona == null)
            throw new NotFoundException("Persona", request.Id);

        // Verificar si tiene usuario asociado
        var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);
        var tieneUsuario = usuarios.Any(u => u.IdPersona == request.Id);

        if (tieneUsuario)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Persona", new[] { "No se puede eliminar la persona porque tiene un usuario asociado" } }
            });
        }

        // Cambiar estado a INACTIVO en lugar de eliminar f�sicamente
        persona.Estado = "INACTIVO";
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}