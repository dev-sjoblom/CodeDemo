using CommunicationService.Classifications.Core;
using CommunicationService.Classifications.Data;
using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using Npgsql;
using static CommunicationService.MetadataTypes.Core.MetadataTypeErrors;

namespace CommunicationService.MetadataTypes.Core;

public class MetadataTypeRepositoryReader : IMetadataTypeRepositoryReader
{
    private const string ixMetadataTypeName = "IX_MetadataType_Name";

    private CommunicationDbContext DbContext { get; }

    public MetadataTypeRepositoryReader(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task<ErrorOr<Deleted>> DeleteMetadataType(Guid id, CancellationToken cancellationToken)
    {
        if (await DbContext.MetadataType.FindAsync(new object?[] { id }, cancellationToken) is not MetadataType metadataType)
            return NotFound(id.ToString());

        DbContext.Remove(metadataType);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }

    public async Task<ErrorOr<MetadataType>> GetMetadataTypeById(Guid id, CancellationToken cancellationToken)
    {
        var task = DbContext.MetadataType
            .Include(x => x.Classifications)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (await task is not MetadataType metadataType)
            return NotFound(id.ToString());

        return metadataType;
    }

    public async Task<ErrorOr<MetadataType>> GetMetadataTypeByName(string name, CancellationToken cancellationToken)
    {
        var task = DbContext.MetadataType
            .Include(x => x.Classifications)
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (await task is not MetadataType metadataType)
            return NotFound(name);

        return metadataType;
    }

    public async Task<ErrorOr<IEnumerable<MetadataType>>> ListMetadataTypes(CancellationToken cancellationToken)
    {
        return await DbContext.MetadataType
            .Include(x => x.Classifications)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<UpsertedMetadataTypeResult>> UpsertMetadataType(MetadataType data, string[] classifications, CancellationToken cancellationToken)
    {
        var registerAsNew = !await DbContext.MetadataType.AnyAsync(x => x.Id == data.Id, cancellationToken: cancellationToken);
        
        var item = data;
        
        if (registerAsNew)
        {
            DbContext.MetadataType.Add(item);
        }
        else
        {
            var existing = await GetMetadataTypeById(data.Id, cancellationToken);
            if (existing.IsError)
                return existing.Errors;
            item = existing.Value;
            item.Name = data.Name; 
            
            DbContext.MetadataType.Update(item);
            
            item.Classifications.RemoveAll(x => true);
        }
        
        foreach (var name in classifications)
        {
            var task = DbContext.Classification.Where(x => x.Name == name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (await task is not Classification classification)
                return ClassificationErrors.NotFound;
            
            if (!item.Classifications.Contains(classification))
                item.Classifications.Add(classification);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
            return new UpsertedMetadataTypeResult(RegisteredAsNewItem: registerAsNew);
        }
        catch (DbUpdateException updateException) when (updateException.InnerException is PostgresException)
        {
            var message = updateException.InnerException.Message;
            if (message.Contains(ixMetadataTypeName))
            {
                return NameAlreadyExists;
            }

            throw;
        }
    }
}