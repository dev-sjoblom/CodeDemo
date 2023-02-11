namespace CommunicationService.Test.ClassificationTests.ContractModels;

public record UpsertClassificationRequest(string Name, string[] MetadataTypes)
{
}