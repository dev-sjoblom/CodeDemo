namespace CommunicationService.Receivers.Data;

public interface IReceiverRepository
{
    Task<ErrorOr<Created>> CreateReceiver(
        Receiver receiver,
        string[] classifications,
        KeyValuePair<string, string>[] metadatas,
        CancellationToken cancellationToken);

    Task<ErrorOr<Receiver>> GetReceiverById(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<Receiver>> GetReceiverByUniqueName(string uniqueName, CancellationToken cancellationToken);

    Task<ErrorOr<UpsertedReceiverResult>> UpsertReceiver(
        Receiver data,
        string[] classifications,
        KeyValuePair<string, string>[] metadataTypes,
        CancellationToken cancellationToken);

    Task<ErrorOr<Deleted>> DeleteReceiver(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<Receiver>>> ListReceivers(CancellationToken cancellationToken);
}