using CommunicationService.MetadataTypes.Data;
using Npgsql;
using static CommunicationService.Classifications.Fundamental.ClassificationErrors;

namespace CommunicationService.Classifications.Data;

public class ClassificationRepositoryReader : IClassificationRepositoryReader
{
    private const string ixClassificationName = "IX_Classification_Name";
    private CommunicationDbContext DbContext { get; }
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    public ClassificationRepositoryReader(CommunicationDbContext dbContext, IMetadataTypeRepositoryReader metadataTypeRepositoryReader)
    {
        DbContext = dbContext;
        MetadataTypeRepositoryReader = metadataTypeRepositoryReader;
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
}