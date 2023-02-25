using CommunicationService.Classifications.Features.GetById;
using CommunicationService.Fundamental.DataAccess;

namespace CommunicationService.Classifications.Features.Delete;

public class DeleteClassificationHandler : IRequestHandler<DeleteClassificationCommand, ErrorOr<Deleted>>
{
    private ILogger Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public DeleteClassificationHandler(
        ILogger<DeleteClassificationHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteClassificationCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existingCommand = new GetClassificationByIdQuery()
        {
            Id = request.Id
        };
        var classificationResult = await Mediator.Send(existingCommand, cancellationToken);

        if (classificationResult.IsError)
            return classificationResult.Errors;

        DbContext.Remove(classificationResult.Value);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}