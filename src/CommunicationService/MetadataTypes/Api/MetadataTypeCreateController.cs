using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Commands;
using FluentValidation;
using MediatR;

namespace CommunicationService.MetadataTypes.Api;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[Route("[controller]")]
public class MetadataTypeCreateController : MetadataTypeBaseController
{
    private IMediator Mediator { get; }
    private IValidator<CreateMetadataTypeRequest> RequestValidator { get; }

    public MetadataTypeCreateController(
        ILogger<MetadataTypeCreateController> logger,
        IMediator mediator,
        IValidator<CreateMetadataTypeRequest> requestValidator
    ) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMetadataType(CreateMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var result = await Mediator.Send(new CreateMetadataTypeCommand()
        {
            Name = request.Name,
            Classifications = request.Classifications
        }, cancellationToken);

        return result.Match(
            CreatedAtMetadataType,
            Problem);
    }
}