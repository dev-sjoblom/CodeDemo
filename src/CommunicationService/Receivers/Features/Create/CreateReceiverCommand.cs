using CommunicationService.Receivers.DataStore;
using MediatR;

namespace CommunicationService.Receivers.Features.Create;

public class CreateReceiverCommand : IRequest<ErrorOr<Receiver>>
{
    public required string UniqueName { get; init; }
    public required string Email { get; init; }
    public required string[] Classifications { get; init; }
    public required KeyValuePair<string, string>[] Metadatas { get; init; }
}