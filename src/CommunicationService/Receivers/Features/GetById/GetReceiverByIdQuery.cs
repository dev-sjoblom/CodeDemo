using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Features.GetById;

public class GetReceiverByIdQuery : IRequest<ErrorOr<Receiver>>
{
    public Guid Id { get; init; }
}