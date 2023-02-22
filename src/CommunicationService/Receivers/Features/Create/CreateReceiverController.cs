using CommunicationService.Receivers.Fundamental;
using FluentValidation;
using MediatR;

namespace CommunicationService.Receivers.Features.Create;

[Route( Route )]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class CreateReceiverController : ReceiverBase
{
    private IMediator Mediator { get; }
    private IValidator<CreateReceiverRequest> RequestValidator { get; }

    public CreateReceiverController(
        ILogger<CreateReceiverController> logger,
        IMediator mediator,
        IValidator<CreateReceiverRequest> requestValidator
    ) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    /// <summary>
    /// Create a new receiver
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostReceiver(CreateReceiverRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var command = new CreateReceiverCommand()
        {
            UniqueName = request.UniqueName,
            Email = request.Email,
            Classifications = request.Classifications,
            Metadatas = request.Metadata
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            CreatedAtReceiver,
            Problem);
    }
}