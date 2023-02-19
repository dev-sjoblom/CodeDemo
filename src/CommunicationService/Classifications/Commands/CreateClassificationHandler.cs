using CommunicationService.Classifications.Data;
using CommunicationService.Classifications.Queries;
using MediatR;

namespace CommunicationService.Classifications.Commands;

public class CreateClassificationHandler : IRequestHandler<CreateClassificationCommand, ErrorOr<Classification>>
{
    private ILogger Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public CreateClassificationHandler(
        ILogger<CreateClassificationHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<Classification>> Handle(CreateClassificationCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Name);
        ArgumentNullException.ThrowIfNull(request.MetadataTypes);

        var existingResult = await Mediator.Send(
            new GetClassificationByNameQuery() { Name = request.Name },
            cancellationToken);

        if (!existingResult.IsError)
            return ClassificationCommandErrors.NameAlreadyExists;
        if (existingResult.FirstError != ClassificationQueryErrors.NotFound)
            return existingResult.Errors;

        var upsertResult = await Mediator.Send(new UpsertClassificationCommand()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            MetadataTypes = request.MetadataTypes
        }, cancellationToken);

        if (upsertResult.IsError)
            return upsertResult.Errors;

        return upsertResult.Value.Classification;
    }
}