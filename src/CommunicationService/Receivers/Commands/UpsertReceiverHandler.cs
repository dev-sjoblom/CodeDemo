using CommunicationService.Classifications.Queries;
using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Queries;
using CommunicationService.MetadataTypes.Queries;
using MediatR;

namespace CommunicationService.Receivers.Commands;

public class UpsertReceiverHandler : IRequestHandler<UpsertReceiverCommand, ErrorOr<UpsertReceiverCommandResult>>
{
    private const string IxReceiverName = "IX_Receiver_UniqueName";

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

        var receiverResult =
            await Mediator.Send(new GetReceiverByIdQuery() { Id = request.Id }, cancellationToken);
        var registerAsNew = false;

        if (receiverResult.IsError)
        {
            if (receiverResult.FirstError.Type != ErrorType.NotFound)
                return receiverResult.Errors;
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
            receiver = receiverResult.Value;

            receiver.UniqueName = request.UniqueName;
            receiver.Email = request.Email;

            DbContext.Receiver.Update(receiver);
        }

        receiver.Classifications.RemoveAll(x => true);
        foreach (var classificationName in request.Classifications)
        {
            var classificationResult =
                await Mediator.Send(new GetClassificationByNameQuery() { Name = classificationName },
                    cancellationToken);
            if (classificationResult.IsError)
                return classificationResult.Errors;
            receiver.Classifications.Add(classificationResult.Value);
        }

        receiver.Metadatas.RemoveAll(x => true);
        foreach (var metadataItem in request.Metadatas)
        {
            var metadataType =
                await Mediator.Send(new GetMetadataTypeByNameQuery() { Name = metadataItem.Key }, cancellationToken);

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
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(IxReceiverName))
        {
            return ReceiverCommandErrors.NameAlreadyExists;
        }
    }
}