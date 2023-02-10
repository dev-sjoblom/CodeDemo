namespace CommunicationService.MetadataTypes;

public static class MetadataTypeErrors
{
    public static Error InvalidName => Error.Validation(
        code: "MetadataType.InvalidName",
        description: $"MetadataType name must be at least {Models.MetadataType.MinNameLength}" +
                     $" characters long and at most {Models.MetadataType.MaxNameLength} characters long.");

    public static Error NameAlreadyExists => Error.Conflict(
        code: "MetadataType.NameAlreadyExists",
        description: $"MetadataType name already taken.");
    
    public static Error NotFound(string filter) => Error.NotFound(
        code: "MetadataType.NotFound",
        description: $"{filter} was not found.");
    
}