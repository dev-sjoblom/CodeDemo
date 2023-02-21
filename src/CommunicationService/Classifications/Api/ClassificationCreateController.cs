using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Commands;
using FluentValidation;
using MediatR;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class ClassificationCreateController : ClassificationBaseController
{
    private IMediator Mediator { get; }
    private IValidator<CreateClassificationRequest> RequestValidator { get; }

    public ClassificationCreateController(
        ILogger<ClassificationCreateController> logger,
        IMediator mediator,
        IValidator<CreateClassificationRequest> requestValidator) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClassification(CreateClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var result = await Mediator.Send(new CreateClassificationCommand()
        {
            Name = request.Name,
            MetadataTypes = request.MetadataTypes
        }, cancellationToken);

        return result.Match(
            CreatedAtClassification,
            Problem);
    }
}