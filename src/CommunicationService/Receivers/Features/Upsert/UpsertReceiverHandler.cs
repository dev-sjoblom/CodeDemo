using CommunicationService.Classifications.Features.GetByName;
using CommunicationService.MetadataTypes.Features.GetByName;
using CommunicationService.Receivers.DataStore;
using CommunicationService.Receivers.Features.GetById;
using CommunicationService.Receivers.Fundamental;
using MediatR;

namespace CommunicationService.Receivers.Features.Upsert;

public class UpsertReceiverHandler : IRequestHandler<UpsertReceiverCommand, ErrorOr<UpsertReceiverCommandResult>>
{
    private ILogger Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public UpsertReceiverHandler(
        ILogger<UpsertReceiverHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<UpsertReceiverCommandResult>> Handle(UpsertReceiverCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.UniqueName);
        ArgumentNullException.ThrowIfNull(request.Email);
        ArgumentNullException.ThrowIfNull(request.Classifications);
        ArgumentNullException.ThrowIfNull(request.Metadatas);

        var existingCommand = new GetReceiverByIdQuery()
        {
            Id = request.Id
        };
        var existingReceiverResult = await Mediator.Send(existingCommand, cancellationToken);
        var registerAsNew = false;

        if (existingReceiverResult.IsError)
        {
            if (existingReceiverResult.FirstError.Type != ErrorType.NotFound)
                return existingReceiverResult.Errors;
            registerAsNew = true;
        }

        Receiver receiver;
        if (registerAsNew)
        {
            receiver = new Receiver
            {
                Id = request.Id,
                UniqueName = request.UniqueName,
                Email = request.Email
            };

            DbContext.Receiver.Add(receiver);
        }
        else
        {
            receiver = existingReceiverResult.Value;

            receiver.UniqueName = request.UniqueName;
            receiver.Email = request.Email;

            DbContext.Receiver.Update(receiver);
        }

        receiver.Classifications.RemoveAll(x => true);
        foreach (var classificationName in request.Classifications)
        {
            var existingClassificationCommand = new GetClassificationByNameQuery()
            {
                Name = classificationName
            };
            var classificationResult =
                await Mediator.Send(existingClassificationCommand, cancellationToken);
            if (classificationResult.IsError)
                return classificationResult.Errors;
            receiver.Classifications.Add(classificationResult.Value);
        }

        receiver.Metadatas.RemoveAll(x => true);
        foreach (var metadataItem in request.Metadatas)
        {
            var existingMetadataTypeCommand = new GetMetadataTypeByNameQuery()
            {
                Name = metadataItem.Key
            };
            var metadataType = await Mediator.Send(existingMetadataTypeCommand, cancellationToken);

            if (metadataType.IsError)
                return metadataType.Errors;

            if (!metadataType.Value.Classifications.Any(x => receiver.Classifications.Any(p => x.Id == p.Id)))
                return ReceiverCommandErrors.MetadataTypeNotAllowed;

            var metadata = new ReceiverMetadata()
            {
                MetadataType = metadataType.Value,
                Receiver = receiver,
                Data = metadataItem.Value
            };

            receiver.Metadatas.Add(metadata);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);

            return new UpsertReceiverCommandResult()
            {
                RegisteredAsNewItem = registerAsNew,
                Receiver = receiver
            };
        }
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(ReceiverIndex.IxReceiverName))
        {
            return ReceiverCommandErrors.NameAlreadyExists;
        }
    }
}