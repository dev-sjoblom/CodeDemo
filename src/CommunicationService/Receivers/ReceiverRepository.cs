using CommunicationService.Classifications;
using CommunicationService.Classifications.DataModels;
using CommunicationService.MetadataTypes;
using CommunicationService.MetadataTypes.Models;
using CommunicationService.Receivers.DataModels;
using CommunicationService.Receivers.Models;
using static CommunicationService.Receivers.ReceiverErrors;

namespace CommunicationService.Receivers;

public class ReceiverRepository : IReceiverRepository
{
    private CommunicationDbContext DbContext { get; }
    private IClassificationRepository ClassificationRepository { get; }
    private IMetadataTypeRepository MetadataTypeRepository { get; }

    public ReceiverRepository(CommunicationDbContext dbContext, 
        IClassificationRepository classificationRepository,
        IMetadataTypeRepository metadataTypeRepository)
    {
        DbContext = dbContext;
        ClassificationRepository = classificationRepository;
        MetadataTypeRepository = metadataTypeRepository;
    }

    public async Task<ErrorOr<Created>> CreateReceiver(
        Receiver receiver, 
        string[] classifications, 
        KeyValuePair<string, string>[] metadatas,
        CancellationToken cancellationToken)
    {
        DbContext.Receiver.Add(receiver);

        receiver.Metadatas.RemoveAll(x => true);
        foreach (var item in metadatas)
        {
            var task = DbContext.MetadataType.Where(x => x.Name == item.Key)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (await task is not MetadataType metadataType)
                return NotFound;
            
            var metadata = new ReceiverMetadata()
            {
                MetadataType = metadataType,
                Receiver = receiver,
                Data = item.Value
            };
            
            receiver.Metadatas.Add(metadata);
        }
        
        receiver.Classifications.RemoveAll(x => true);
        foreach (var name in classifications)
        {
            var task = DbContext.Classification.Where(x => x.Name == name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (await task is not Classification classification)
                return NotFound;
            
            receiver.Classifications.Add(classification);
        }        
        
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteReceiver(Guid id, CancellationToken cancellationToken)
    {
        if (await DbContext.Receiver.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) is not Receiver receiver)
            return NotFound;

        DbContext.Remove(receiver);
        
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }

    public async Task<ErrorOr<Receiver>> GetReceiverById(Guid id, CancellationToken cancellationToken)
    {
        var receiver = await DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (receiver is null)
            return NotFound;

        return receiver;
    }

    public async Task<ErrorOr<Receiver>> GetReceiverByUniqueName(string uniqueName, CancellationToken cancellationToken)
    {
        var task = DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .Where(x => x.UniqueName == uniqueName)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (await task is not Receiver classification)
            return NotFound;

        return classification;
    }

    public async Task<ErrorOr<IEnumerable<Receiver>>> ListReceivers(CancellationToken cancellationToken)
    {
        return await DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .OrderBy(x => x.UniqueName)
            .ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<UpsertedReceiverResult>> UpsertReceiver(
        Receiver data, 
        string[] classifications,
        KeyValuePair<string, string>[] metadataTypes,
        CancellationToken cancellationToken)
    {
        var registerAsNew = !await DbContext.Receiver.AnyAsync(x => x.Id == data.Id, cancellationToken: cancellationToken);
        
        var receiver = data;
        
        if (registerAsNew)
        {
            DbContext.Receiver.Add(receiver);
        }
        else
        {
            var existing = await GetReceiverById(data.Id, cancellationToken);
            if (existing.IsError)
                return existing.Errors;
            
            receiver = existing.Value;
            
            receiver.UniqueName = data.UniqueName;
            receiver.Email = data.Email;
            
            DbContext.Receiver.Update(receiver);
        }
        
        receiver.Classifications.RemoveAll(x=> true);
        foreach (var c in classifications)
        {
            var classification = await ClassificationRepository.GetClassificationByName(c, cancellationToken);
            if (classification.IsError)
                return classification.Errors;
            receiver.Classifications.Add(classification.Value);
        }

        receiver.Metadatas.RemoveAll(x => true);
        foreach (var metadataItem in metadataTypes)
        {
            var metadataType = await MetadataTypeRepository.GetMetadataTypeByName(metadataItem.Key, cancellationToken);
            if (metadataType.IsError)
                return metadataType.Errors;
                
            var metadata = new ReceiverMetadata()
            {
                MetadataType = metadataType.Value,
                Receiver = receiver,
                Data = metadataItem.Value
            };
            
            receiver.Metadatas.Add(metadata);
        }

        await DbContext.SaveChangesAsync(cancellationToken);
        
        return new UpsertedReceiverResult(RegisteredAsNewItem: registerAsNew);
    }
}