using CommunicationService.Classifications.Data;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.Receivers.Data;
using Npgsql;
using static CommunicationService.Receivers.Core.ReceiverErrors;

namespace CommunicationService.Receivers.Core;

public class ReceiverRepositoryReader : IReceiverRepositoryReader
{
    private CommunicationDbContext DbContext { get; }
    private IClassificationRepositoryReader ClassificationRepositoryReader { get; }
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }
    private const string ixReceiverName = "IX_Receiver_UniqueName";

    public ReceiverRepositoryReader(CommunicationDbContext dbContext, 
        IClassificationRepositoryReader classificationRepositoryReader,
        IMetadataTypeRepositoryReader metadataTypeRepositoryReader)
    {
        DbContext = dbContext;
        ClassificationRepositoryReader = classificationRepositoryReader;
        MetadataTypeRepositoryReader = metadataTypeRepositoryReader;
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
}