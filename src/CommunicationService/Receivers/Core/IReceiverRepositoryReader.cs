using CommunicationService.Receivers.Data;

namespace CommunicationService.Receivers.Core;

public interface IReceiverRepositoryReader
{
    Task<ErrorOr<Receiver>> GetReceiverByUniqueName(string uniqueName, CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<Receiver>>> ListReceivers(CancellationToken cancellationToken);
    Task<ErrorOr<Receiver>> GetReceiverById(Guid id, CancellationToken cancellationToken);
}