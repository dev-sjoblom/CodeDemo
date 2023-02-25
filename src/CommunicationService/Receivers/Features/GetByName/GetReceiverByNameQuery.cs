using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Features.GetByName;

public class GetReceiverByNameQuery : IRequest<ErrorOr<Receiver>>
{
    public required string UniqueName { get; init; }
}