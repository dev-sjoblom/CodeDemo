using CommunicationService.Classifications.Fundamental;
using FluentValidation;
using MediatR;

namespace CommunicationService.Classifications.Features.Upsert;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(ClassificationResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public class UpsertClassificationController : ClassificationBase
{
    private IMediator Mediator { get; }
    private IValidator<UpsertClassificationRequest> RequestValidator { get; }

    public UpsertClassificationController(
        ILogger<UpsertClassificationController> logger,
        IMediator mediator,
        IValidator<UpsertClassificationRequest> RequestValidator) : base(logger)
    {
        Mediator = mediator;
        this.RequestValidator = RequestValidator;
    }

    /// <summary>
    /// Upsert a classification
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertClassification(Guid id,
        UpsertClassificationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var command = new UpsertClassificationCommand()
        {
            Id = id,
            Name = request.Name,
            MetadataTypes = request.MetadataTypes
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(item => item.RegisteredAsNewItem
                ? CreatedAtClassification(item.Classification)
                : NoContent(),
            Problem);
    }
}