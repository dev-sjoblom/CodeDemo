using MediatR;

namespace CommunicationService.Classifications.Commands;

public class DeleteClassificationCommand : IRequest<ErrorOr<Deleted>>
{
    public required Guid Id { get; init; }
}