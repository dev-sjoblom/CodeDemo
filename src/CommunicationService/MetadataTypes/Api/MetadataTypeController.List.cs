namespace CommunicationService.MetadataTypes;

public partial class MetadataTypeController
{
    [HttpGet]
    public async Task<IActionResult> ListMetadataTypes(CancellationToken cancellationToken)
    {
        var metadataTypesResult = await MetadataTypeRepository.ListMetadataTypes(cancellationToken);

        return metadataTypesResult.Match(
            onValue: item => Ok(item.Select(x =>
                x.ToMetadataTypeResponse())),
            onError: Problem);
    }
}