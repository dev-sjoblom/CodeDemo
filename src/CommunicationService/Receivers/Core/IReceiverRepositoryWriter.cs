using CommunicationService.Receivers.Data;

namespace CommunicationService.Receivers.Core;

public interface IReceiverRepositoryWriter
{
    Task<ErrorOr<Created>> CreateReceiver(
        Receiver receiver,
        string[] classifications,
        KeyValuePair<string, string>[] metadatas,
        CancellationToken cancellationToken);

    Task<ErrorOr<UpsertedReceiverResult>> UpsertReceiver(
        Receiver data,
        string[] classifications,
        KeyValuePair<string, string>[] metadataTypes,
        CancellationToken cancellationToken);

    Task<ErrorOr<Deleted>> DeleteReceiver(Guid id, CancellationToken cancellationToken);
}