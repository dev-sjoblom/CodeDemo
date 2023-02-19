using CommunicationService.Receivers.Data;

namespace CommunicationService.Receivers.Commands;

public class UpsertReceiverCommandResult
{
    public required bool RegisteredAsNewItem { get; init; }
    public required Receiver Receiver { get; init; }
}