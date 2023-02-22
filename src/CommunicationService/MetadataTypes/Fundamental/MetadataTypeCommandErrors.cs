namespace CommunicationService.MetadataTypes.Fundamental;

public static class MetadataTypeCommandErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        "MetadataType.NameAlreadyExists",
        $"MetadataType name already taken.");
}