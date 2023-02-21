using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Commands;
using FluentValidation;
using MediatR;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[Route("[controller]")]
public class ReceiverUpsertController : ReceiverBaseController
{
    private IMediator Mediator { get; }
    private IValidator<UpsertReceiverRequest> RequestValidator { get; }

    public ReceiverUpsertController(
        ILogger<ReceiverUpsertController> logger,
        IMediator mediator,
        IValidator<UpsertReceiverRequest> requestValidator) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertReceiver(Guid id, UpsertReceiverRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var result = await Mediator.Send(new UpsertReceiverCommand()
        {
            Id = id,
            UniqueName = request.UniqueName,
            Email = request.Email,
            Classifications = request.Classifications,
            Metadatas = request.Metadata
        }, cancellationToken);

        return result.Match(
            item => item.RegisteredAsNewItem ? CreatedAtReceiver(item.Receiver) : NoContent(),
            Problem);
    }
}