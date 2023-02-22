using MediatR;

namespace CommunicationService.Receivers.Features.Upsert;

public class UpsertReceiverCommand : IRequest<ErrorOr<UpsertReceiverCommandResult>>
{
    public required Guid Id { get; init; }
    public required string UniqueName { get; init; }
    public required string Email { get; init; }

    public required string[] Classifications { get; init; }
    public required KeyValuePair<string, string>[] Metadatas { get; init; }
}