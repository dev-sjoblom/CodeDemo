using Newtonsoft.Json;

namespace CommunicationService.Test.ClassificationTests.Response;

public static class ClassificationResponseValidator
{
    public static async Task<ClassificationResponseItem> ValidateClassificationResponse(
        HttpResponseMessage responseMessage,
        HttpStatusCode expectedStatusCode)
    {
        var stringContent = await responseMessage.Content.ReadAsStringAsync();

        responseMessage.StatusCode.Should().Be(expectedStatusCode, stringContent);
        var classificationResponseItem = JsonConvert.DeserializeObject<ClassificationResponseItem>(stringContent)!;
        classificationResponseItem.Id.Should().NotBeEmpty(stringContent);
        classificationResponseItem.Name.Should().NotBeNull(stringContent);
        classificationResponseItem.MetadataTypes.Should().NotBeNull(stringContent);
        return classificationResponseItem;
    }
}