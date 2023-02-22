namespace CommunicationService.MetadataTypes.Fundamental;

public static class MetadataTypeQueryErrors
{
    public static Error NotFound => Error.NotFound(
        code: "MetadataType.NotFound",
        description: $"MetadataType was not found.");
}