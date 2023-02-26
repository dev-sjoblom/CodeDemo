namespace CommunicationService.Test.Fundamental.Response;

public class ProblemResponse
{
    public required string Title { get; init; }
    public required string Status { get; init; }
    public required string TraceId { get; init; }
    public IDictionary<string, object>? Errors { get; } = new Dictionary<string, object>(StringComparer.Ordinal);
}