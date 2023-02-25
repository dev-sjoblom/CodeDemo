namespace CommunicationService.Receivers.Features.Delete;

public class DeleteReceiverCommand : IRequest<ErrorOr<Deleted>>
{
    public required Guid Id { get; init; }
}