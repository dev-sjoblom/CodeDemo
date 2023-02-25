using CommunicationService.Classifications.DataAccess;
using CommunicationService.Classifications.Features.GetByName;
using CommunicationService.Classifications.Features.Upsert;
using CommunicationService.Classifications.Fundamental;
using CommunicationService.Fundamental.DataAccess;

namespace CommunicationService.Classifications.Features.Create;

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

        var checkExistingCommand = new GetClassificationByNameQuery()
        {
            Name = request.Name
        };
        var existingResult = await Mediator.Send(checkExistingCommand, cancellationToken);

        if (!existingResult.IsError)
            return ClassificationErrors.NameAlreadyExists;
        if (existingResult.FirstError != ClassificationErrors.NotFound)
            return existingResult.Errors;

        var upsertCommand = new UpsertClassificationCommand()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            MetadataTypes = request.MetadataTypes
        };
        var upsertResult = await Mediator.Send(upsertCommand, cancellationToken);

        if (upsertResult.IsError)
            return upsertResult.Errors;

        return upsertResult.Value.Classification;
    }
}