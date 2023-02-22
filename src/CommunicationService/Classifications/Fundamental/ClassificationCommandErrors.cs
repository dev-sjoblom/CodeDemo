namespace CommunicationService.Classifications.Fundamental;

public static class ClassificationCommandErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        "Classification.NameAlreadyExists",
        $"Classification name already taken.");
}