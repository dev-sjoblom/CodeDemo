using Newtonsoft.Json;

namespace CommunicationService.Test.ReceiversTests.Response;

public static class ReceiverResponseValidator
{
    public static async Task<ReceiverResponseItem> ValidateReceiverResponse(
        HttpResponseMessage responseMessage,
        HttpStatusCode expectedStatusCode)
    {
        responseMessage.StatusCode.Should().Be(expectedStatusCode);
        var stringContent = await responseMessage.Content.ReadAsStringAsync();
        var metadataResponseItem = JsonConvert.DeserializeObject<ReceiverResponseItem>(stringContent)!;
        metadataResponseItem.Id.Should().NotBeEmpty();
        metadataResponseItem.UniqueName.Should().NotBeNull();
        metadataResponseItem.Email.Should().NotBeNull();
        metadataResponseItem.Classifications.Should().NotBeNull();
        metadataResponseItem.Metadatas.Should().NotBeNull();
        
        return metadataResponseItem;
    }
}