using CommunicationService.MetadataTypes.Features.GetById;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.Delete;

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

        var existingCommand = new GetMetadataTypeByIdQuery()
        {
            Id = request.Id
        };
        var existingResult = await Mediator.Send(existingCommand, cancellationToken);

        if (existingResult.IsError)
            return existingResult.Errors;

        DbContext.Remove(existingResult.Value);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}