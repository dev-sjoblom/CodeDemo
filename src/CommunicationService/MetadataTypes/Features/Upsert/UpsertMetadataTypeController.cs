using CommunicationService.MetadataTypes.Fundamental;
using FluentValidation;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.Upsert;

[Route( Route)]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public class UpsertMetadataTypeController : MetadataTypeBase
{
    private IMediator Mediator { get; }
    private IValidator<UpsertMetadataTypeRequest> RequestValidator { get; }

    public UpsertMetadataTypeController(
        ILogger<UpsertMetadataTypeController> logger,
        IMediator mediator,
        IValidator<UpsertMetadataTypeRequest> requestValidator
    ) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    /// <summary>
    /// Upsert a Metadata Type
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertMetadataType(Guid id, UpsertMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var command = new UpsertMetadataTypeCommand()
        {
            Id = id,
            Classifications = request.Classifications,
            Name = request.Name
        };
        
        var result = await Mediator.Send(command, cancellationToken);

        return result.Match(
            item => item.RegisteredAsNewItem ? CreatedAtMetadataType(item.MetadataType) : NoContent(),
            Problem);
    }
}