using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Commands;
using FluentValidation;
using MediatR;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[Route("[controller]")]
public class MetadataTypeUpsertController : MetadataTypeBaseController
{
    private IMediator Mediator { get; }
    private IValidator<UpsertMetadataTypeRequest> RequestValidator { get; }

    public MetadataTypeUpsertController(
        ILogger<MetadataTypeUpsertController> logger,
        IMediator mediator,
        IValidator<UpsertMetadataTypeRequest> requestValidator
    ) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertMetadataType(Guid id, UpsertMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var result = await Mediator.Send(
            new UpsertMetadataTypeCommand()
            {
                Id = id,
                Classifications = request.Classifications,
                Name = request.Name
            }, cancellationToken);

        return result.Match(
            item => item.RegisteredAsNewItem ? CreatedAtMetadataType(item.MetadataType) : NoContent(),
            Problem);
    }
}