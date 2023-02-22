namespace CommunicationService.Classifications.Fundamental;

public static class ClassificationQueryErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Classification.NotFound",
        description: "Classification not found");

}