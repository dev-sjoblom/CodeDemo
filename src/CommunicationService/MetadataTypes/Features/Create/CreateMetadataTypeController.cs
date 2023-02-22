using CommunicationService.MetadataTypes.Fundamental;
using FluentValidation;
using MediatR;

namespace CommunicationService.MetadataTypes.Features.Create;


[Route( "MetadataType")]
[ApiExplorerSettings(GroupName = GroupNaming)]
[Produces("application/json")]
[ProducesResponseType(typeof(MetadataTypeResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class CreateMetadataTypeController : MetadataTypeBase
{
    private IMediator Mediator { get; }
    private IValidator<CreateMetadataTypeRequest> RequestValidator { get; }

    public CreateMetadataTypeController(
        ILogger<CreateMetadataTypeController> logger,
        IMediator mediator,
        IValidator<CreateMetadataTypeRequest> requestValidator
    ) : base(logger)
    {
        Mediator = mediator;
        RequestValidator = requestValidator;
    }

    /// <summary>
    /// Creates a new Metadata Type
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateMetadataType(CreateMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await RequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ValidationProblem(validationResult);

        var commands = new CreateMetadataTypeCommand()
        {
            Name = request.Name,
            Classifications = request.Classifications
        };
        
        var result = await Mediator.Send(commands, cancellationToken);

        return result.Match(
            CreatedAtMetadataType,
            Problem);
    }
}