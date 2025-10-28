using MediatR;

namespace Miski.Application.Features.Maestros.Banco.Commands.DeleteBanco;

public record DeleteBancoCommand(int Id) : IRequest<Unit>;
