using CommunicationService.Fundamental.DataAccess;
using CommunicationService.MetadataTypes.DataAccess;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Features.GetById;

public class GetMetadataTypeByIdHandler : IRequestHandler<GetMetadataTypeByIdQuery, ErrorOr<MetadataType>>
{
    private CommunicationDbContext DbContext { get; }

    public GetMetadataTypeByIdHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<MetadataType>> Handle(GetMetadataTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var metadataType = await DbContext.MetadataType
            .Include(x => x.Classifications)
            .Where(x => x.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (metadataType is null)
            return MetadataTypeErrors.NotFound;
        return metadataType;
    }
}