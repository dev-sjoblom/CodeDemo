using CommunicationService.Receivers.Queries;
using MediatR;

namespace CommunicationService.Receivers.Commands;

public class DeleteReceiverHandler : IRequestHandler<DeleteReceiverCommand, ErrorOr<Deleted>>
{
    private ILogger Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public DeleteReceiverHandler(
        ILogger<DeleteReceiverHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteReceiverCommand request,
        CancellationToken cancellationToken)
    {
        var classificationResult = await Mediator.Send(
            new GetReceiverByIdQuery() { Id = request.Id },
            cancellationToken);

        if (classificationResult.IsError)
            return classificationResult.Errors;

        DbContext.Remove(classificationResult.Value);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}