namespace CommunicationService.Test.Fundamental.Response;

public static class ResponseValidator
{
    public static async Task<string> ValidateResponse(
        HttpResponseMessage responseMessage,
        HttpStatusCode expectedStatusCode)
    {
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        responseMessage.StatusCode.Should().Be(
            expectedStatusCode, 
            $"ExpectedStatusCode: {expectedStatusCode}, response: {responseContent}");

        return responseContent;
    }
}