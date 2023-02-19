using MediatR;

namespace CommunicationService.Receivers.Commands;

public class DeleteReceiverCommand : IRequest<ErrorOr<Deleted>>
{
    public required Guid Id { get; init; }
}