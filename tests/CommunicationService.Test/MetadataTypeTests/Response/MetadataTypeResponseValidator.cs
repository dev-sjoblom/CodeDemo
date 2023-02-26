using Newtonsoft.Json;

namespace CommunicationService.Test.MetadataTypeTests.Response;

public static class MetadataTypeResponseValidator
{
    public static async Task<MetadataTypeResponseItem> ValidateMetadataResponse(
        HttpResponseMessage responseMessage,
        HttpStatusCode expectedStatusCode)
    {
        responseMessage.StatusCode.Should().Be(expectedStatusCode);
        var stringContent = await responseMessage.Content.ReadAsStringAsync();
        var metadataResponseItem = JsonConvert.DeserializeObject<MetadataTypeResponseItem>(stringContent)!;
        metadataResponseItem.Id.Should().NotBeEmpty();
        metadataResponseItem.Name.Should().NotBeNull();
        metadataResponseItem.Classifications.Should().NotBeNull();
        
        return metadataResponseItem;
    }
}