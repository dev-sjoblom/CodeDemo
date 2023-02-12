namespace CommunicationService.Classifications.Api.Model;

public record UpsertClassificationRequest(string Name, string[] MetadataTypes)
{
}