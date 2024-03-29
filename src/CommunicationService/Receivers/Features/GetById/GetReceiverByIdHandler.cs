using CommunicationService.Fundamental.DataAccess;
using CommunicationService.Receivers.DataAccess;
using CommunicationService.Receivers.Fundamental;

namespace CommunicationService.Receivers.Features.GetById;

public class GetReceiverByIdHandler : IRequestHandler<GetReceiverByIdQuery, ErrorOr<Receiver>>
{
    private CommunicationDbContext DbContext { get; }

    public GetReceiverByIdHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<Receiver>> Handle(GetReceiverByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var receiver = await DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (receiver is null)
            return ReceiverErrors.NotFound;
        return receiver;
    }
}