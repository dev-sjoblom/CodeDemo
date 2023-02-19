using CommunicationService.Receivers.Data;
using MediatR;

namespace CommunicationService.Receivers.Commands;

public class CreateReceiverCommand : IRequest<ErrorOr<Receiver>>
{
    public required string UniqueName { get; init; }
    public required string Email { get; init; }
    public required string[] Classifications { get; init; }
    public required KeyValuePair<string, string>[] Metadatas { get; init; }
}