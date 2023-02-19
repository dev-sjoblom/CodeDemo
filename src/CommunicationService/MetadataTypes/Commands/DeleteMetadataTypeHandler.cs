using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.MetadataTypes.Commands;

public class DeleteMetadataTypeHandler : IRequestHandler<DeleteMetadataTypeCommand, ErrorOr<Deleted>>
{
    private ILogger Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public DeleteMetadataTypeHandler(
        ILogger<DeleteMetadataTypeHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteMetadataTypeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var classificationResult = await Mediator.Send(
            new GetMetadataTypeByIdQuery() { Id = request.Id },
            cancellationToken);

        if (classificationResult.IsError)
            return classificationResult.Errors;

        DbContext.Remove(classificationResult.Value);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}