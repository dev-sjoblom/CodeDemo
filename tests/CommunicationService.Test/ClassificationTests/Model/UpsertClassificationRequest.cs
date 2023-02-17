namespace CommunicationService.Test.ClassificationTests.Model;

public record UpsertClassificationRequest(string Name, string[] MetadataTypes)
{
}