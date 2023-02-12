namespace CommunicationService.Classifications.Data;

public interface IClassificationRepository
{
    Task<ErrorOr<Created>> CreateClassification(Classification classification, string[] metadataTypes, CancellationToken cancellationToken);
    Task<ErrorOr<Classification>> GetClassificationById(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<Classification>> GetClassificationByName(string name, CancellationToken cancellationToken);
    Task<ErrorOr<UpsertedClassificationResult>> UpsertClassification(Classification data, string[] metadataTypes, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> DeleteClassification(Guid id, CancellationToken cancellationToken);
    Task<ErrorOr<IEnumerable<Classification>>> ListClassifications(CancellationToken cancellationToken);
}