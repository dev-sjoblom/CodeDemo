using CommunicationService.MetadataTypes.Data;
using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.MetadataTypes.Commands;

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


        var existingResult = await Mediator.Send(
            new GetMetadataTypeByNameQuery() { Name = request.Name },
            cancellationToken);

        if (!existingResult.IsError)
            return MetadataTypeCommandErrors.NameAlreadyExists;
        if (existingResult.FirstError != MetadataTypeQueryErrors.NotFound)
            return existingResult.Errors;

        var upsertResult = await Mediator.Send(new UpsertMetadataTypeCommand()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Classifications = request.Classifications
        }, cancellationToken);

        if (upsertResult.IsError)
            return upsertResult.Errors;

        return upsertResult.Value.MetadataType;
    }
}