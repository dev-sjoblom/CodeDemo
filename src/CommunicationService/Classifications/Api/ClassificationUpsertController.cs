using CommunicationService.Classifications.Api.Model;
using CommunicationService.Classifications.Commands;
using FluentValidation;
using MediatR;

namespace CommunicationService.Classifications.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[Route("[controller]")]
public class ClassificationUpsertController : ClassificationBaseController
{
    private IMediator Mediator { get; }
    private IValidator<UpsertClassificationRequest> RequestValidator { get; }

    public ClassificationUpsertController(
        ILogger<ClassificationUpsertController> logger,
        IMediator mediator,
        IValidator<UpsertClassificationRequest> RequestValidator) : base(logger)
    {
        Mediator = mediator;
        this.RequestValidator = RequestValidator;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertClassification(Guid id,
        UpsertClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var result = await Mediator.Send(new UpsertClassificationCommand()
        {
            Id = id,
            Name = request.Name,
            MetadataTypes = request.MetadataTypes
        }, cancellationToken);

        return result.Match(item => item.RegisteredAsNewItem
                ? CreatedAtClassification(item.Classification)
                : NoContent(),
            Problem);
    }
}