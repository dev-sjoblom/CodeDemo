using CommunicationService.Classifications.Data;
using static CommunicationService.Receivers.Core.ReceiverErrors;

namespace CommunicationService.Receivers.Data;

public class Receiver
{
    public const int MinNameLength = 1;
    public const int MaxNameLength = 100;
    public Guid Id { get; private set; }
    public string UniqueName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<Classification> Classifications { get; set; }
    public List<ReceiverMetadata> Metadatas { get; set; }

    private Receiver()
    {
        Classifications = new();
        Metadatas = new ();
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
        {
            errors.Add(InvalidName);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Receiver(uniqueName, email, id: id);
    }

}