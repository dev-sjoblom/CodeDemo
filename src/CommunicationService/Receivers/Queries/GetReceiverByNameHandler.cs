using CommunicationService.Receivers.Data;
using MediatR;

namespace CommunicationService.Receivers.Queries;

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
            return ReceiverQueryErrors.NotFound;

        return receiver;
    }
}