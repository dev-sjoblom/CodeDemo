using CommunicationService.Receivers.Fundamental;
using FluentValidation;
using MediatR;

namespace CommunicationService.Receivers.Features.Upsert;

[Route( Route )]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public class UpsertReceiverController : ReceiverBase
{
    private IMediator Mediator { get; }
    private IValidator<UpsertReceiverRequest> RequestValidator { get; }

    public UpsertReceiverController(
        ILogger<UpsertReceiverController> logger,
        IMediator mediator,
        IValidator<UpsertReceiverRequest> requestValidator) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }


    /// <summary>
    /// Upsert a receiver.
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Upsert(Guid id, UpsertReceiverRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var command = new UpsertReceiverCommand()
        {
            Id = id,
            UniqueName = request.UniqueName,
            Email = request.Email,
            Classifications = request.Classifications,
            Metadatas = request.Metadata
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            onValue: item => item.RegisteredAsNewItem ? 
                CreatedAtReceiver(item.Receiver) : NoContent(),
            onError: Problem);
    }
}