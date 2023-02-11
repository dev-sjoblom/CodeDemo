using CommunicationService.Classifications.DataModels;

namespace CommunicationService.Classifications;

public static class ClassificationErrors
{
    public static Error InvalidNameLength => Error.Validation(
        code: "Classification.InvalidNameLength",
        description: $"Classification name must be at least {Classification.MinNameLength}" +
                     $" characters long and at most {Classification.MaxNameLength} characters long.");

    public static Error InvalidName => Error.Validation(
        code: "Classification.InvalidName",
        description: $"Classification name can only contain letters between a - z");

    public static Error NameAlreadyExists => Error.Conflict(
        code: "Classification.NameAlreadyExists",
        description: $"Classification name already taken.");
    
    public static Error NotFound => Error.NotFound(
        code: "Classification.NotFound",
        description: "Classification not found");
    
    public static Error InvalidMetadataType(string metadataType) => Error.NotFound(
        code: "Classification.InvalidMetadataType",
        description: $"{metadataType} was not found.");

}