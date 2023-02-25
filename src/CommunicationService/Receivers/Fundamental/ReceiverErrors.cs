using CommunicationService.Fundamental.Errors;
using CommunicationService.MetadataTypes.DataAccess;
using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Fundamental;

public static class ReceiverErrors
{
    private const string _MetadataTypeNotAllowed = "MetadataTypeNotAllowed";
    
    public static Error NotFound => 
        ErrorHelper.NotFoundError(ReceiverConstants.Receiver);
    
    public static Error NameAlreadyExists => 
        ErrorHelper.NameAlreadyTakenError(ReceiverConstants.Receiver); 

    public static Error MetadataTypeNotAllowed => Error.Custom(StatusCodes.Status424FailedDependency,
        $"{ReceiverConstants.Receiver}.{_MetadataTypeNotAllowed}",
        $"{MetadataTypeConstants.MetadataType} not allowed.");
}