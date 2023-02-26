using Newtonsoft.Json;

namespace CommunicationService.Test.Fundamental.Response;

public static class ResponseProblemHelper
{
    public const string ValidationProblemTitle = "One or more validation errors occurred.";

    public static string WasNotFoundTitle(string entityName) =>
        $"{entityName} was not found.";
    
    public static async Task ValidateResponseProblem(
        HttpResponseMessage responseMessage,
        HttpStatusCode expectedStatusCode,
        string? withTitle = null,
        string? withErrorResponseKey = null,
        string? withErrorResponseMessage = null)
    {
        var responseContent = await ValidateResponse(responseMessage, expectedStatusCode);

        var problemResponse = JsonConvert.DeserializeObject<ProblemResponse?>(responseContent)!;
        
        problemResponse.Should().NotBeNull(responseContent);
        problemResponse.Title.Should().NotBeNullOrWhiteSpace(responseContent);
        problemResponse.Status.Should().NotBeNullOrWhiteSpace(responseContent);
        problemResponse.TraceId.Should().NotBeNullOrWhiteSpace(responseContent);
        
        if (withTitle != null)
            problemResponse.Title.Should().Contain(
            withTitle,
            responseContent);

        if (withErrorResponseKey != null)
            problemResponse.Errors.Should().ContainKey(
                withErrorResponseKey,
                $"Error response key {withErrorResponseKey} not found in response: {responseContent}");

        if (withErrorResponseMessage != null)
            problemResponse?.Errors?.Values.Should().Contain(x => x != null && x.ToString()!.Contains(withErrorResponseMessage)); 
    }
}