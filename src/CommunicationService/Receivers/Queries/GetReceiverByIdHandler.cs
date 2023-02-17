using CommunicationService.Receivers.Data;
using MediatR;

namespace CommunicationService.Receivers.Queries;

public class GetReceiverByIdHandler : IRequestHandler<GetReceiverByIdQuery, ErrorOr<Receiver>>
{
    private CommunicationDbContext DbContext { get; }

    public GetReceiverByIdHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<Receiver>> Handle(GetReceiverByIdQuery request, CancellationToken cancellationToken)
    {
        var receiver = await DbContext.Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .ThenInclude(x => x.MetadataType)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (receiver is null)
            return ReceiverQueryErrors.NotFound;
        return receiver;
    }
}