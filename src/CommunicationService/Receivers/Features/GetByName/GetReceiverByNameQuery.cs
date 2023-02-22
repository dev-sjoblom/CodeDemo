using CommunicationService.Receivers.DataStore;
using MediatR;

namespace CommunicationService.Receivers.Features.GetByName;

public class GetReceiverByNameQuery : IRequest<ErrorOr<Receiver>>
{
    public required string UniqueName { get; init; }
}