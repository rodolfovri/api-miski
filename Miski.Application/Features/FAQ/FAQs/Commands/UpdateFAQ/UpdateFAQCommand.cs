using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.FAQs.Commands.UpdateFAQ;

public record UpdateFAQCommand(int Id, UpdateFAQDto FAQ) : IRequest<FAQDto>;
