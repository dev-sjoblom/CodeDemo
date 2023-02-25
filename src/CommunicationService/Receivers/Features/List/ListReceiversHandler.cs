using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Features.List;

public class ListReceiversHandler : IRequestHandler<ListReceiversQuery, ErrorOr<IEnumerable<Receiver>>>
{
    private CommunicationDbContext DbContext { get; }

    public ListReceiversHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<IEnumerable<Receiver>>> Handle(ListReceiversQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .OrderBy(x => x.UniqueName)
            .ToListAsync(cancellationToken);
    }
}