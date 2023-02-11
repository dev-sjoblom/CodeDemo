namespace CommunicationService.Classifications.ContractModels;

public record UpsertClassificationRequest(string Name, string[] MetadataTypes)
{
}