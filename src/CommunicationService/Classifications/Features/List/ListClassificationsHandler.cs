using CommunicationService.Classifications.DataAccess;
using CommunicationService.Fundamental.DataAccess;

namespace CommunicationService.Classifications.Features.List;

public class ListClassificationsHandler : IRequestHandler<ListClassificationsQuery, ErrorOr<IEnumerable<Classification>>>
{
    private CommunicationDbContext DbContext { get; }

    public ListClassificationsHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<IEnumerable<Classification>>> Handle(ListClassificationsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var classification = await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .ToListAsync(cancellationToken);

        return classification;
    }
}