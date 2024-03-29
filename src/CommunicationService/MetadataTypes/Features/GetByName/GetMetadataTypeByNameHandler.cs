using CommunicationService.Fundamental.DataAccess;
using CommunicationService.MetadataTypes.DataAccess;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Features.GetByName;

public class GetMetadataTypeByNameHandler : IRequestHandler<GetMetadataTypeByNameQuery, ErrorOr<MetadataType>>
{
    private CommunicationDbContext DbContext { get; }

    public GetMetadataTypeByNameHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<MetadataType>> Handle(GetMetadataTypeByNameQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Name);

        var metadataType = await DbContext.MetadataType
            .Include(x => x.Classifications)
            .Where(x => x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (metadataType is null)
            return MetadataTypeErrors.NotFound;
        return metadataType;
    }
}