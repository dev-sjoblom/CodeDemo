using CommunicationService.Classifications.Data;
using MediatR;

namespace CommunicationService.Classifications.Queries;

public class GetClassificationsHandler : IRequestHandler<GetClassificationsQuery, ErrorOr<IEnumerable<Classification>>>
{
    private CommunicationDbContext DbContext { get; }

    public GetClassificationsHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<IEnumerable<Classification>>> Handle(GetClassificationsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var classification = await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .ToListAsync(cancellationToken);

        return classification;
    }
}