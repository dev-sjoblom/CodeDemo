using CommunicationService.Receivers.Api.Model;
using CommunicationService.Receivers.Commands;
using FluentValidation;
using MediatR;

namespace CommunicationService.Receivers.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ReceiverResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ReceiverCreateController : ReceiverBaseController
{
    private IMediator Mediator { get; }
    private IValidator<CreateReceiverRequest> RequestValidator { get; }

    public ReceiverCreateController(
        ILogger<ReceiverCreateController> logger,
        IMediator mediator,
        IValidator<CreateReceiverRequest> requestValidator
    ) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReceiver(CreateReceiverRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var result = await Mediator.Send(new CreateReceiverCommand()
        {
            UniqueName = request.UniqueName,
            Email = request.Email,
            Classifications = request.Classifications,
            Metadatas = request.Metadata
        }, cancellationToken);

        return result.Match(
            CreatedAtReceiver,
            Problem);
    }
}