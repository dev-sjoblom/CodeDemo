using CommunicationService.MetadataTypes.Api.DataContract;

namespace CommunicationService.MetadataTypes.Api;

public partial class MetadataTypeController
{
    [HttpPost]
    public async Task<IActionResult> CreateMetadataType(CreateMetadataTypeRequest request, CancellationToken cancellationToken)
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
}