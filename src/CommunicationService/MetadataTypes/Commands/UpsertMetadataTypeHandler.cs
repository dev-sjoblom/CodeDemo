using CommunicationService.Classifications.Queries;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.MetadataTypes.Commands;

public class UpsertMetadataTypeHandler : IRequestHandler<UpsertMetadataTypeCommand,
    ErrorOr<UpsertMetadataTypeCommandResult>>
{
    private const string IxMetadataTypeName = "IX_MetadataType_Name";

    private ILogger Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public UpsertMetadataTypeHandler(
        ILogger<UpsertMetadataTypeHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<UpsertMetadataTypeCommandResult>> Handle(UpsertMetadataTypeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Name);
        ArgumentNullException.ThrowIfNull(request.Classifications);

        var metadataTypeResult =
            await Mediator.Send(new GetMetadataTypeByIdQuery() { Id = request.Id }, cancellationToken);
        var registerAsNew = false;

        if (metadataTypeResult.IsError)
        {
            if (metadataTypeResult.FirstError.Type != ErrorType.NotFound)
                return metadataTypeResult.Errors;
            registerAsNew = true;
        }

        MetadataType metadataType;
        if (registerAsNew)
        {
            metadataType = new MetadataType()
            {
                Id = request.Id,
                Name = request.Name
            };

            DbContext.MetadataType.Add(metadataType);
        }
        else
        {
            metadataType = metadataTypeResult.Value;
            metadataType.Name = request.Name;
            DbContext.MetadataType.Update(metadataType);
        }

        metadataType.Classifications.RemoveAll(x => true);
        foreach (var name in request.Classifications)
        {
            var classificationResult = await Mediator.Send(
                new GetClassificationByNameQuery() { Name = name },
                cancellationToken);

            if (classificationResult.IsError)
                return classificationResult.Errors;

            metadataType.Classifications.Add(classificationResult.Value);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);

            return new UpsertMetadataTypeCommandResult()
            {
                RegisteredAsNewItem = registerAsNew,
                MetadataType = metadataType
            };
        }
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(IxMetadataTypeName))
        {
            return MetadataTypeCommandErrors.NameAlreadyExists;
        }
    }
}