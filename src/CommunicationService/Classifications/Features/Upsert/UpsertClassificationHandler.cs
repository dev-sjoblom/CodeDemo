using CommunicationService.Classifications.DataStore;
using CommunicationService.Classifications.Features.GetById;
using CommunicationService.Classifications.Fundamental;
using CommunicationService.MetadataTypes.Features.GetByName;
using MediatR;

namespace CommunicationService.Classifications.Features.Upsert;

public class UpsertClassificationHandler : IRequestHandler<UpsertClassificationCommand,
    ErrorOr<UpsertClassificationCommandResult>>
{

    private ILogger Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public UpsertClassificationHandler(
        ILogger<UpsertClassificationHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<UpsertClassificationCommandResult>> Handle(UpsertClassificationCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Name);
        ArgumentNullException.ThrowIfNull(request.MetadataTypes);

        var existingClassificationCommand = new GetClassificationByIdQuery()
        {
            Id = request.Id
        };
        var classificationResult = await Mediator.Send(existingClassificationCommand, cancellationToken);
        var registerAsNew = false;

        if (classificationResult.IsError)
        {
            if (classificationResult.FirstError.Type != ErrorType.NotFound)
                return classificationResult.Errors;
            registerAsNew = true;
        }

        Classification classification;
        if (registerAsNew)
        {
            classification = new Classification()
            {
                Id = request.Id,
                Name = request.Name
            };

            DbContext.Classification.Add(classification);
        }
        else
        {
            classification = classificationResult.Value;

            classification.Name = request.Name;
            DbContext.Classification.Update(classification);
        }

        classification.MetadataTypes.RemoveAll(x => true);
        foreach (var name in request.MetadataTypes)
        {
            var existingMetadataTypeCommand = new GetMetadataTypeByNameQuery()
            {
                Name = name
            };
            var metadataResult = await Mediator.Send(existingMetadataTypeCommand, cancellationToken);

            if (metadataResult.IsError)
                return metadataResult.Errors;

            classification.MetadataTypes.Add(metadataResult.Value);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);

            return new UpsertClassificationCommandResult()
            {
                RegisteredAsNewItem = registerAsNew,
                Classification = classification
            };
        }
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(ClassificationIndex.IxClassificationName))
        {
            return ClassificationCommandErrors.NameAlreadyExists;
        }
    }
}