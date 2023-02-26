using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CommunicationService.Test.Fundamental.Helpers;

public static class RestSerializeHelper
{
    private static readonly MediaTypeHeaderValue JsonHeader = new("application/json");

    public static string Serialize(object value) => JsonSerializer.Serialize(value);

    public static StringContent? AsJsonStringContent (this object? value) =>
        value == null
            ? null
            : new StringContent(Serialize((object)value))
            {
                Headers = { ContentType = JsonHeader }
            };
}