namespace CommunicationService.Test.ReceiversTests.Model;

public class CreateReceiverRequestParameters
{
    public string? UniqueName { get; init; }
    public string? Email { get; init; }
    public string[]? Classifications { get; init; }
    public KeyValuePair<string, string>[]? Metadata { get; init; }
}