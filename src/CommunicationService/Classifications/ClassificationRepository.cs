using CommunicationService.Classifications.DataModels;
using CommunicationService.Classifications.Models;
using CommunicationService.MetadataTypes.Models;
using Npgsql;
using static CommunicationService.Classifications.ClassificationErrors;

namespace CommunicationService.Classifications;

public class ClassificationRepository : IClassificationRepository
{
    private const string ixClassificationName = "IX_Classification_Name";
    private CommunicationDbContext DbContext { get; }

    public ClassificationRepository(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<Created>> CreateClassification(Classification classification, string[] metadataTypes,
        CancellationToken cancellationToken)
    {
        DbContext.Classification.Add(classification);

        foreach (var name in metadataTypes)
        {
            var task = DbContext.MetadataType.Where(x => x.Name == name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (await task is not MetadataType metadataType)
                return InvalidMetadataType(name);
            
            classification.MetadataTypes.Add(metadataType);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
            
            return Result.Created;
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

    public async Task<ErrorOr<Deleted>> DeleteClassification(Guid id, CancellationToken cancellationToken)
    {
        var classification = DbContext.Classification.FirstOrDefault(x => x.Id == id);
        if (classification is null)
            return NotFound;

        DbContext.Remove(classification);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }

    public async Task<ErrorOr<Classification>> GetClassificationById(Guid id, CancellationToken cancellationToken)
    {
        var task = DbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (await task is not Classification classification)
            return NotFound;

        return classification;
    }

    public async Task<ErrorOr<Classification>> GetClassificationByName(string name, CancellationToken cancellationToken)
    {
        var task = DbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (await task is not Classification classification)
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
        var registerAsNew = !await DbContext.Classification.AnyAsync(x => x.Id == data.Id, cancellationToken: cancellationToken);
        
        var classification = data;
        
        if (registerAsNew)
        {
            DbContext.Classification.Add(classification);
        }
        else
        {
            var existing = await GetClassificationById(data.Id, cancellationToken);
            if (existing.IsError)
                return existing.Errors;
            classification = existing.Value;
            classification.Name = data.Name; 
            
            DbContext.Classification.Update(classification);

        }

        classification.MetadataTypes.RemoveAll(x => true);
        foreach (var name in metadataTypes)
        {
            var task = DbContext.MetadataType.Where(x => x.Name == name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (await task is not MetadataType metadataType)
                return NotFound;
            
            classification.MetadataTypes.Add(metadataType);
        }
        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        
            return new UpsertedClassificationResult(RegisteredAsNewItem: registerAsNew);
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