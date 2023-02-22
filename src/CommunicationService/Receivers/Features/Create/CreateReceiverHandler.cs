using CommunicationService.Receivers.DataStore;
using CommunicationService.Receivers.Features.GetByName;
using CommunicationService.Receivers.Features.Upsert;
using CommunicationService.Receivers.Fundamental;
using MediatR;

namespace CommunicationService.Receivers.Features.Create;

public class CreateReceiverHandler : IRequestHandler<CreateReceiverCommand, ErrorOr<Receiver>>
{
    private ILogger<CreateReceiverHandler> Logger { get; }
    private CommunicationDbContext DbContext { get; }
    private IMediator Mediator { get; }

    public CreateReceiverHandler(
        ILogger<CreateReceiverHandler> logger,
        CommunicationDbContext dbContext,
        IMediator mediator)
    {
        Logger = logger;
        DbContext = dbContext;
        Mediator = mediator;
    }

    public async Task<ErrorOr<Receiver>> Handle(CreateReceiverCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.UniqueName);
        ArgumentNullException.ThrowIfNull(request.Email);
        ArgumentNullException.ThrowIfNull(request.Classifications);
        ArgumentNullException.ThrowIfNull(request.Metadatas);

        var existingResult = await Mediator.Send(
            new GetReceiverByNameQuery() { UniqueName = request.UniqueName },
            cancellationToken);

        if (!existingResult.IsError)
            return ReceiverCommandErrors.NameAlreadyExists;
        if (existingResult.FirstError != ReceiverQueryErrors.NotFound)
            return existingResult.Errors;

        var upsertCommand = new UpsertReceiverCommand()
        {
            Id = Guid.NewGuid(),
            UniqueName = request.UniqueName,
            Email = request.Email,
            Classifications = request.Classifications,
            Metadatas = request.Metadatas
        };
        var upsertResult = await Mediator.Send(upsertCommand, cancellationToken);

        if (upsertResult.IsError)
            return upsertResult.Errors;

        return upsertResult.Value.Receiver;
    }
}