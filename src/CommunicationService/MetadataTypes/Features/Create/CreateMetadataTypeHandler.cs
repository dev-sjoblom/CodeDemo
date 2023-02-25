using CommunicationService.Fundamental.DataAccess;
using CommunicationService.MetadataTypes.DataAccess;
using CommunicationService.MetadataTypes.Features.GetByName;
using CommunicationService.MetadataTypes.Features.Upsert;
using CommunicationService.MetadataTypes.Fundamental;

namespace CommunicationService.MetadataTypes.Features.Create;

public class CreateMetadataTypeHandler : IRequestHandler<CreateMetadataTypeCommand, ErrorOr<MetadataType>>
{
    private ILogger<CreateMetadataTypeHandler> Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public CreateMetadataTypeHandler(
        ILogger<CreateMetadataTypeHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<MetadataType>> Handle(CreateMetadataTypeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Name);
        ArgumentNullException.ThrowIfNull(request.Classifications);
        
        var existingMetadataTypeCommand = new GetMetadataTypeByNameQuery()
        {
            Name = request.Name
        };
        var existingResult = await Mediator.Send(existingMetadataTypeCommand, cancellationToken);

        if (!existingResult.IsError)
            return MetadataTypeErrors.NameAlreadyExists;
        if (existingResult.FirstError != MetadataTypeErrors.NotFound)
            return existingResult.Errors;

        var upsertMetadataTypeCommand  = new UpsertMetadataTypeCommand()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Classifications = request.Classifications
        };
        
        var upsertResult = await Mediator.Send(upsertMetadataTypeCommand , cancellationToken);

        if (upsertResult.IsError)
            return upsertResult.Errors;

        return upsertResult.Value.MetadataType;
    }
}