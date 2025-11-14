using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociacionesByUsuario;

public record GetNegociacionesByUsuarioQuery(int IdUsuario) : IRequest<List<NegociacionDto>>;
