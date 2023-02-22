using CommunicationService.Classifications.Fundamental;
using FluentValidation;
using MediatR;

namespace CommunicationService.Classifications.Features.Create;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class CreateClassificationController : ClassificationBase
{
    private IMediator Mediator { get; }
    private IValidator<CreateClassificationRequest> RequestValidator { get; }

    public CreateClassificationController(
        ILogger<CreateClassificationController> logger,
        IMediator mediator,
        IValidator<CreateClassificationRequest> requestValidator) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    /// <summary>
    /// Creates a new classification
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostClassification(CreateClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var command = new CreateClassificationCommand()
        {
            Name = request.Name,
            MetadataTypes = request.MetadataTypes
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            CreatedAtClassification,
            Problem);
    }
}