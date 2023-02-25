using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Receivers.DataAccess;
using CommunicationService.Receivers.Fundamental;

namespace CommunicationService.Receivers.Features.GetByName;

public class GetReceiverByNameHandler : IRequestHandler<GetReceiverByNameQuery, ErrorOr<Receiver>>
{
    private CommunicationDbContext DbContext { get; }

    public GetReceiverByNameHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<Receiver>> Handle(GetReceiverByNameQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.UniqueName);

        var receiver = await DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .Where(x => x.UniqueName == request.UniqueName)
            .FirstOrDefaultAsync(cancellationToken);

        if (receiver is null)
            return ReceiverErrors.NotFound;

        return receiver;
    }
}