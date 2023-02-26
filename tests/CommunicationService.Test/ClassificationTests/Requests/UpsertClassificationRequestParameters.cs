namespace CommunicationService.Test.ClassificationTests.Requests;

public record UpsertClassificationRequestParameters(string Name, string[] MetadataTypes)
{
}