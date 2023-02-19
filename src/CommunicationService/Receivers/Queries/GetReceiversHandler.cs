using CommunicationService.Receivers.Data;
using MediatR;

namespace CommunicationService.Receivers.Queries;

public class GetReceiversHandler : IRequestHandler<GetReceiversQuery, ErrorOr<IEnumerable<Receiver>>>
{
    private CommunicationDbContext DbContext { get; }

    public GetReceiversHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<IEnumerable<Receiver>>> Handle(GetReceiversQuery request,
        CancellationToken cancellationToken)
    {
        return await DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .OrderBy(x => x.UniqueName)
            .ToListAsync(cancellationToken);
    }
}