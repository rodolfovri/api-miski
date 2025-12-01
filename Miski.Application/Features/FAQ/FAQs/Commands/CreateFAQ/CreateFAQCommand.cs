using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.FAQs.Commands.CreateFAQ;

public record CreateFAQCommand(CreateFAQDto FAQ) : IRequest<FAQDto>;
