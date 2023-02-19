using CommunicationService.Receivers.Data;
using MediatR;

namespace CommunicationService.Receivers.Queries;

public class GetReceiverByNameQuery : IRequest<ErrorOr<Receiver>>
{
    public required string UniqueName { get; init; }
}