namespace CommunicationService.Classifications.Data;

public interface IClassificationRepositoryReader
{
    Task<ErrorOr<Classification>> GetClassificationById(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<Classification>> GetClassificationByName(string name, CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<Classification>>> ListClassifications(CancellationToken cancellationToken);
}