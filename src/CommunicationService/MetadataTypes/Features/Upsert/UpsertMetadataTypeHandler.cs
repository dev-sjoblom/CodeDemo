using CommunicationService.Classifications.Features.GetByName;
using CommunicationService.Fundamental.DataAccess;
using CommunicationService.MetadataTypes.DataAccess;
using CommunicationService.MetadataTypes.Features.GetById;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Features.Upsert;

public class UpsertMetadataTypeHandler : IRequestHandler<UpsertMetadataTypeCommand,
    ErrorOr<UpsertMetadataTypeCommandResult>>
{
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

        var existingMetadataTypeCommand = new GetMetadataTypeByIdQuery()
        {
            Id = request.Id
        };
        var existingMetadataTypeResult = await Mediator.Send(existingMetadataTypeCommand, cancellationToken);
        
        var registerAsNew = false;
        if (existingMetadataTypeResult.IsError)
        {
            if (existingMetadataTypeResult.FirstError.Type != ErrorType.NotFound)
                return existingMetadataTypeResult.Errors;
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
            metadataType = existingMetadataTypeResult.Value;
            metadataType.Name = request.Name;
            DbContext.MetadataType.Update(metadataType);
        }

        metadataType.Classifications.RemoveAll(x => true);
        foreach (var name in request.Classifications)
        {
            var existingClassificationCommand = new GetClassificationByNameQuery()
            {
                Name = name
            };
            var classificationResult = await Mediator.Send(existingClassificationCommand, cancellationToken);

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
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(MetadataTypeConstants.IxMetadataTypeName))
        {
            return MetadataTypeErrors.NameAlreadyExists;
        }
    }
}