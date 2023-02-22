using CommunicationService.MetadataTypes.DataStore;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.Get;

public class ListMetadataTypesHandler : IRequestHandler<ListMetadataTypesQuery, ErrorOr<IEnumerable<MetadataType>>>
{
    private CommunicationDbContext DbContext { get; }

    public ListMetadataTypesHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<IEnumerable<MetadataType>>> Handle(ListMetadataTypesQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var metadataType = await DbContext.MetadataType
            .Include(x => x.Classifications)
            .ToListAsync(cancellationToken);

        return metadataType;
    }
}