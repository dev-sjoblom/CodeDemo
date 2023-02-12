using CommunicationService.MetadataTypes.Api.DataContract;

namespace CommunicationService.MetadataTypes.Api;

public partial class MetadataTypeController
{
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertMetadataType(Guid id, UpsertMetadataTypeRequest request,
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
}