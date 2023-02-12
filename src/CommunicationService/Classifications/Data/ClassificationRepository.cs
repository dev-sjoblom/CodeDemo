using CommunicationService.MetadataTypes.Data;
using Npgsql;
using static CommunicationService.Classifications.Fundamental.ClassificationErrors;

namespace CommunicationService.Classifications.Data;

public class ClassificationRepository : IClassificationRepository
{
    private const string ixClassificationName = "IX_Classification_Name";
    private CommunicationDbContext DbContext { get; }
    private IMetadataTypeRepository MetadataTypeRepository { get; }

    public ClassificationRepository(CommunicationDbContext dbContext, IMetadataTypeRepository metadataTypeRepository)
    {
        DbContext = dbContext;
        MetadataTypeRepository = metadataTypeRepository;
    }

    public async Task<ErrorOr<Created>> CreateClassification(Classification classification, string[] metadataTypes,
        CancellationToken cancellationToken)
    {
        var existingResult = await GetClassificationByName(classification.Name, cancellationToken);
        if (existingResult.IsError && existingResult.FirstError != NotFound)
            return existingResult.Errors;
        if (!existingResult.IsError)
            return NameAlreadyExists;

        var createdResult = await UpsertClassification(classification, metadataTypes, cancellationToken);
        if (createdResult.IsError)
            return createdResult.Errors;

        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var classification = await DbContext.Classification.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (classification is null)
            return NotFound;

        DbContext.Remove(classification);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }

    public async Task<ErrorOr<Classification>> GetClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var classification = await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (classification is null)
            return NotFound;

        return classification;
    }

    public async Task<ErrorOr<Classification>> GetClassificationByName(string name, CancellationToken cancellationToken)
    {
        var classification = await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Name == name)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (classification is null)
            return NotFound;

        return classification;
    }

    public async Task<ErrorOr<IEnumerable<Classification>>> ListClassifications(CancellationToken cancellationToken)
    {
        return await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<UpsertedClassificationResult>> UpsertClassification(
        Classification data, 
        string[] metadataTypes,
        CancellationToken cancellationToken)
    {
        var isCreated = false;
        Classification? classification;
        
        var existingResult = await GetClassificationById(data.Id, cancellationToken);
        switch (existingResult.IsError)
        {
            case true when existingResult.FirstError == NotFound:
                isCreated = true;
                classification = data;
                DbContext.Classification.Add(classification);
                break;
            case true:
                return existingResult.Errors;
            default:
                classification = existingResult.Value;
                classification.Name = data.Name; 
                DbContext.Classification.Update(classification);
                break;
        }
        
        classification.MetadataTypes.RemoveAll(x => true);
        foreach (var name in metadataTypes)
        {
            var metadataResult = await MetadataTypeRepository.GetMetadataTypeByName(name, cancellationToken);
            if (metadataResult.IsError)
                return metadataResult.Errors;
            classification.MetadataTypes.Add(metadataResult.Value);
        }
        
        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        
            return new UpsertedClassificationResult(RegisteredAsNewItem: isCreated);
        }
        catch (DbUpdateException updateException) when (updateException.InnerException is PostgresException)
        {
            var message = updateException.InnerException.Message;
            if (message.Contains(ixClassificationName))
            {
                return NameAlreadyExists;
            }

            throw;
        }
    }
}