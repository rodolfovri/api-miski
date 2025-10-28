using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Banco.Commands.CreateBanco;

public record CreateBancoCommand(CreateBancoDto Banco) : IRequest<BancoDto>;
