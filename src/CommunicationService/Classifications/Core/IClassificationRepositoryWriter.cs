namespace CommunicationService.Classifications.Data;

public interface IClassificationRepositoryWriter
{
    Task<ErrorOr<Created>> CreateClassification(Classification classification, string[] metadataTypes, CancellationToken cancellationToken);
    Task<ErrorOr<UpsertedClassificationResult>> UpsertClassification(Classification data, string[] metadataTypes, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> DeleteClassification(Guid id, CancellationToken cancellationToken);
}