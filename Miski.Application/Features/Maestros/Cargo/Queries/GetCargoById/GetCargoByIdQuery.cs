using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Queries.GetCargoById;

public record GetCargoByIdQuery(int Id) : IRequest<CargoDto>;
