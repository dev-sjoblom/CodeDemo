using CommunicationService.Receivers.Data;
using CommunicationService.Receivers.Queries;
using MediatR;

namespace CommunicationService.Receivers.Commands;

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

    public async Task<ErrorOr<Receiver>> Handle(CreateReceiverCommand request,
        CancellationToken cancellationToken)
    {
        var existingResult = await Mediator.Send(
            new GetReceiverByNameQuery() { UniqueName = request.UniqueName },
            cancellationToken);

        if (!existingResult.IsError)
            return ReceiverCommandErrors.NameAlreadyExists;
        if (existingResult.FirstError != ReceiverQueryErrors.NotFound)
            return existingResult.Errors;

        var upsertResult = await Mediator.Send(new UpsertReceiverCommand()
        {
            Id = Guid.NewGuid(),
            UniqueName = request.UniqueName,
            Email = request.Email,
            Classifications = request.Classifications,
            Metadatas = request.Metadatas
        }, cancellationToken);

        if (upsertResult.IsError)
            return upsertResult.Errors;

        return upsertResult.Value.Receiver;
    }
}