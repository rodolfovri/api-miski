using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Banco.Commands.UpdateBanco;

public record UpdateBancoCommand(int Id, UpdateBancoDto Banco) : IRequest<BancoDto>;
