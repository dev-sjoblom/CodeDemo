using Newtonsoft.Json;

namespace CommunicationService.Test.ClassificationTests.Response;

public static class ClassificationResponseValidator
{
    public static async Task<ClassificationResponseItem> ValidateClassificationResponse(
        HttpResponseMessage responseMessage,
        HttpStatusCode expectedStatusCode)
    {
        responseMessage.StatusCode.Should().Be(expectedStatusCode);
        var stringContent = await responseMessage.Content.ReadAsStringAsync();
        var classificationResponseItem = JsonConvert.DeserializeObject<ClassificationResponseItem>(stringContent)!;
        classificationResponseItem.Id.Should().NotBeEmpty();
        classificationResponseItem.Name.Should().NotBeNull();
        classificationResponseItem.MetadataTypes.Should().NotBeNull();
        return classificationResponseItem;
    }
}