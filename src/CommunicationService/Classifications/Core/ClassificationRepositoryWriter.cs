using CommunicationService.MetadataTypes.Data;
using Npgsql;
using static CommunicationService.Classifications.Fundamental.ClassificationErrors;

namespace CommunicationService.Classifications.Data;

public class ClassificationRepositoryWriter : IClassificationRepositoryWriter
{
    private const string ixClassificationName = "IX_Classification_Name";
    private CommunicationDbContext DbContext { get; }
    private IClassificationRepositoryReader RepositoryReader { get; }
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    public ClassificationRepositoryWriter(CommunicationDbContext dbContext,
        IClassificationRepositoryReader repositoryReader,
        IMetadataTypeRepositoryReader MetadataTypeRepositoryReader
    )
    {
        DbContext = dbContext;
        RepositoryReader = repositoryReader;
        this.MetadataTypeRepositoryReader = MetadataTypeRepositoryReader;
    }

    public async Task<ErrorOr<Created>> CreateClassification(
        Classification classification,
        string[] metadataTypes,
        CancellationToken cancellationToken)
    {
        var existingResult = await RepositoryReader.GetClassificationByName(classification.Name, cancellationToken);
        if (!existingResult.IsError)
            return NameAlreadyExists;
        if (existingResult.FirstError != NotFound)
            return existingResult.Errors;

        var createdResult = await UpsertClassification(classification, metadataTypes, cancellationToken);
        if (createdResult.IsError)
            return createdResult.Errors;

        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var classification = await RepositoryReader.GetClassificationById(id, cancellationToken);
        if (classification.IsError)
            return classification.Errors;

        DbContext.Remove(classification.Value);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }

    public async Task<ErrorOr<UpsertedClassificationResult>> UpsertClassification(
        Classification data,
        string[] metadataTypes,
        CancellationToken cancellationToken)
    {
        var classificationResult = await RepositoryReader.GetClassificationById(data.Id, cancellationToken);
        var registerAsNew = false;

        if (classificationResult.IsError)
        {
            if (classificationResult.FirstError.Type != ErrorType.NotFound)
                return classificationResult.Errors;
            registerAsNew = true;
        }

        var classification = registerAsNew ? data : classificationResult.Value;

        if (registerAsNew)
        {
            DbContext.Classification.Add(classification);
        }
        else
        {
            classification.Name = classification.Name;
            DbContext.Classification.Update(classification);
        }

        classification.MetadataTypes.RemoveAll(x => true);
        foreach (var name in metadataTypes)
        {
            var metadataResult = await MetadataTypeRepositoryReader.GetMetadataTypeByName(name, cancellationToken);
            if (metadataResult.IsError)
                return metadataResult.Errors;
            classification.MetadataTypes.Add(metadataResult.Value);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);

            return new UpsertedClassificationResult(registerAsNew);
        }
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(ixClassificationName))
        {
            return NameAlreadyExists;
        }
    }
}