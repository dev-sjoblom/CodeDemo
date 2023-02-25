using CommunicationService.Classifications.DataAccess;
using CommunicationService.Classifications.Fundamental;
using CommunicationService.Fundamental.DataAccess;

namespace CommunicationService.Classifications.Features.GetById;

public class GetClassificationByIdHandler : IRequestHandler<GetClassificationByIdQuery, ErrorOr<Classification>>
{
    private CommunicationDbContext DbContext { get; }

    public GetClassificationByIdHandler(CommunicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<ErrorOr<Classification>> Handle(GetClassificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var classification = await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (classification is null)
            return ClassificationErrors.NotFound;
        return classification;
    }
}