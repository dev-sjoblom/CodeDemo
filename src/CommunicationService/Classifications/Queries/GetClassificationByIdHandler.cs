using CommunicationService.Classifications.Data;
using MediatR;

namespace CommunicationService.Classifications.Queries;

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
        var classification = await DbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (classification is null)
            return ClassificationQueryErrors.NotFound;
        return classification;
    }
}