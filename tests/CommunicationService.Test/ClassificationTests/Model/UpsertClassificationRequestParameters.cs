namespace CommunicationService.Test.ClassificationTests.Model;

public record UpsertClassificationRequestParameters(string Name, string[] MetadataTypes)
{
}