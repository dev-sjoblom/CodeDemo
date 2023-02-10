using CommunicationService.Classifications;
using CommunicationService.MetadataTypes.Contracts;
using CommunicationService.MetadataTypes.Models;

namespace CommunicationService.MetadataTypes;

[ApiController]
[Route("[controller]")]
public class MetadataTypeController : ApiController
{
    private IMetadataTypeRepository MetadataTypeRepository { get; }

    public MetadataTypeController(IMetadataTypeRepository metadataTypeRepository,
        IClassificationRepository classificationRepository)
    {
        MetadataTypeRepository = metadataTypeRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMetadataType(CreateMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var metadataTypeResult = request.ToMetadataType();
        if (metadataTypeResult.IsError)
        {
            return Problem(metadataTypeResult.Errors);
        }

        var metadataType = metadataTypeResult.Value;

        var createMetadataTypeResult = await
            MetadataTypeRepository.CreateMetadataType(metadataType, request.Classifications, cancellationToken);

        return createMetadataTypeResult.Match(
            _ => CreatedAtGetMetadataType(metadataType),
            Problem);
    }

    [HttpGet]
    public async Task<IActionResult> ListMetadataTypes(CancellationToken cancellationToken)
    {
        var metadataTypesResult = await MetadataTypeRepository.ListMetadataTypes(cancellationToken);

        return metadataTypesResult.Match(
            onValue: item => Ok(item.Select(x =>
                x.ToMetadataTypeResponse())),
            onError: Problem);
    }

    [HttpGet("ById/{id:guid}")]
    public async Task<IActionResult> GetMetadataTypeById(Guid id, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await MetadataTypeRepository.GetMetadataTypeById(id, cancellationToken);

        return metadataTypeResult.Match(
            item => Ok(item.ToMetadataTypeResponse()), 
            Problem);
    }

    [HttpGet("ByName/{name}")]
    public async Task<IActionResult> GetMetadataTypeById(string name, CancellationToken cancellationToken)
    {
        var metadataTypeResult = await MetadataTypeRepository.GetMetadataTypeByName(name, cancellationToken);

        return metadataTypeResult.Match(
            item => Ok(item.ToMetadataTypeResponse()),
            Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertBreakfast(Guid id, UpsertMetadataTypeRequest request,
        CancellationToken cancellationToken)
    {
        var requestToMetadataTypeResult = request.ToMetadataType(id);

        if (requestToMetadataTypeResult.IsError) 
            return Problem(requestToMetadataTypeResult.Errors);

        var metadataType = requestToMetadataTypeResult.Value;
        var upsertedResult = await 
            MetadataTypeRepository.UpsertMetadataType(metadataType, request.Classifications, cancellationToken);

        return upsertedResult.Match(
            item => item.RegisteredAsNewItem ? CreatedAtGetMetadataType(metadataType) : NoContent(),
            Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBreakfast(Guid id, CancellationToken cancellationToken)
    {
        var deleteResult = await MetadataTypeRepository.DeleteMetadataType(id, cancellationToken);

        return deleteResult.Match(_ => NoContent(), Problem);
    }

    private CreatedAtActionResult CreatedAtGetMetadataType(MetadataType metadataType) => CreatedAtAction(
        actionName: nameof(GetMetadataTypeById),
        routeValues: new { id = metadataType.Id },
        value: metadataType.ToMetadataTypeResponse());
}