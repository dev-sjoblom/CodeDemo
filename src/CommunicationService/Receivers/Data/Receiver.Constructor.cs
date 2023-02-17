using CommunicationService.Classifications.Data;
using static CommunicationService.Receivers.Commands.ReceiverCommandErrors;

namespace CommunicationService.Receivers.Data;

public partial class Receiver
{
    private Receiver()
    {
        Classifications = new List<Classification>();
        Metadatas = new List<ReceiverMetadata>();
    }

    private Receiver(string uniqueName, string email, Guid? id = null) : this()
    {
        UniqueName = uniqueName;
        Email = email;
        Id = id ?? Guid.NewGuid();
    }

    public static ErrorOr<Receiver> Create(
        string uniqueName,
        string email,
        Guid? id = null)
    {
        List<Error> errors = new();

        if (uniqueName.Length is < MinNameLength or > MaxNameLength)
            errors.Add(InvalidName);

        if (errors.Count > 0)
            return errors;

        return new Receiver(uniqueName, email, id);
    }
}