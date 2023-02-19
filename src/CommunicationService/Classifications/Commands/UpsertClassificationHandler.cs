using CommunicationService.Classifications.Data;
using CommunicationService.Classifications.Queries;
using CommunicationService.Fundamental.Helpers;
using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.Classifications.Commands;

public class UpsertClassificationHandler : IRequestHandler<UpsertClassificationCommand,
    ErrorOr<UpsertClassificationCommandResult>>
{
    private const string IxClassificationName = "IX_Classification_Name";

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

        var classificationResult =
            await Mediator.Send(new GetClassificationByIdQuery() { Id = request.Id }, cancellationToken);
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
            var registerClassificationResult = Classification.Create(request.Name, request.Id);
            if (registerClassificationResult.IsError)
                return registerClassificationResult.Errors;

            classification = registerClassificationResult.Value;

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
            var metadataResult = await Mediator.Send(
                new GetMetadataTypeByNameQuery() { Name = name },
                cancellationToken);

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
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(IxClassificationName))
        {
            return ClassificationCommandErrors.NameAlreadyExists;
        }
    }
}