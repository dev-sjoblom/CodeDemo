using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Features.Upsert;

public class UpsertReceiverCommandResult
{
    public required bool RegisteredAsNewItem { get; init; }
    public required Receiver Receiver { get; init; }
}