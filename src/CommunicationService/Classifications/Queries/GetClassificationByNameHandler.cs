using CommunicationService.Classifications.Data;
using MediatR;

namespace CommunicationService.Classifications.Queries;

public class GetClassificationByNameHandler : IRequestHandler<GetClassificationByNameQuery, ErrorOr<Classification>>
{
    private CommunicationDbContext DbContext { get; }

    public GetClassificationByNameHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<Classification>> Handle(GetClassificationByNameQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Name);

        var classification = await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Name == request.Name)
            .SingleOrDefaultAsync(cancellationToken);

        if (classification is null)
            return ClassificationQueryErrors.NotFound;
        return classification;
    }
}