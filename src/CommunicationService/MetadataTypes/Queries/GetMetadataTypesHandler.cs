using CommunicationService.MetadataTypes.Data;
using MediatR;

namespace CommunicationService.MetadataTypes.Queries;

public class GetMetadataTypesHandler : IRequestHandler<GetMetadataTypesQuery, ErrorOr<IEnumerable<MetadataType>>>
{
    private CommunicationDbContext DbContext { get; }

    public GetMetadataTypesHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<IEnumerable<MetadataType>>> Handle(GetMetadataTypesQuery request,
        CancellationToken cancellationToken)
    {
        var metadataType = await DbContext.MetadataType
            .Include(x => x.Classifications)
            .ToListAsync(cancellationToken);

        return metadataType;
    }
}