using CommunicationService.Classifications.Core;
using CommunicationService.Fundamental.Helpers;
using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using static CommunicationService.MetadataTypes.Core.MetadataTypeErrors;

namespace CommunicationService.MetadataTypes.Core;

public class MetadataTypeRepositoryWriter : IMetadataTypeRepositoryWriter
{
    private const string ixMetadataTypeName = "IX_MetadataType_Name";

    private CommunicationDbContext DbContext { get; }
    private IMetadataTypeRepositoryReader RepositoryReader { get; }
    private IClassificationRepositoryReader ClassificationRepositoryReader { get; }

    public MetadataTypeRepositoryWriter(CommunicationDbContext dbContext,
        IMetadataTypeRepositoryReader repositoryReader,
        IClassificationRepositoryReader classificationRepositoryReader)
    {
        DbContext = dbContext;
        RepositoryReader = repositoryReader;
        ClassificationRepositoryReader = classificationRepositoryReader;
    }

    public async Task<ErrorOr<Created>> CreateMetadataType(MetadataType metadataType, string[] classifications,
        CancellationToken cancellationToken)
    {
        var existingResult = await RepositoryReader.GetMetadataTypeByName(metadataType.Name, cancellationToken);
        if (!existingResult.IsError)
            return NameAlreadyExists;
        if (existingResult.FirstError.Type != ErrorType.NotFound)
            return existingResult.Errors;

        var createdResult = await UpsertMetadataType(metadataType, classifications, cancellationToken);
        if (createdResult.IsError)
            return createdResult.Errors;

        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteMetadataType(Guid id, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await RepositoryReader.GetMetadataTypeById(id, cancellationToken);
        if (metadataTypeResult.IsError)
            return metadataTypeResult.Errors;

        DbContext.Remove(metadataTypeResult.Value);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }

    public async Task<ErrorOr<UpsertedMetadataTypeResult>> UpsertMetadataType(MetadataType data,
        string[] classifications, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await RepositoryReader.GetMetadataTypeById(data.Id, cancellationToken);
        var registerAsNew = false;

        if (metadataTypeResult.IsError)
        {
            if (metadataTypeResult.FirstError.Type != ErrorType.NotFound)
                return metadataTypeResult.Errors;
            registerAsNew = true;
        }

        var metadataType = registerAsNew ? data : metadataTypeResult.Value;

        if (registerAsNew)
        {
            DbContext.MetadataType.Add(metadataType);
        }
        else
        {
            metadataType.Name = data.Name;
            DbContext.MetadataType.Update(metadataType);
        }

        metadataType.Classifications.RemoveAll(x => true);
        foreach (var name in classifications)
        {
            var classificationResult =
                await ClassificationRepositoryReader.GetClassificationByName(name, cancellationToken);
            if (classificationResult.IsError)
                return classificationResult.Errors;

            if (!metadataType.Classifications.Contains(classificationResult.Value))
                metadataType.Classifications.Add(classificationResult.Value);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
            return new UpsertedMetadataTypeResult(registerAsNew);
        }
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(ixMetadataTypeName))
        {
            return NameAlreadyExists;
        }
    }
}