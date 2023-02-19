using CommunicationService.Classifications.Data;
using CommunicationService.MetadataTypes.Commands;
using static System.Char;

namespace CommunicationService.MetadataTypes.Data;

public partial class MetadataType
{
    private MetadataType()
    {
        Classifications = new List<Classification>();
    }

    private MetadataType(string name, Guid? id = null)
    {
        Name = name;
        Id = id ?? Guid.NewGuid();
        Classifications = new List<Classification>();
    }

    public static ErrorOr<MetadataType> Create(
        string name,
        Guid? id = null)
    {
        List<Error> errors = new();

        if (name.Length is < MinNameLength or > MaxNameLength) errors.Add(MetadataTypeCommandErrors.InvalidName);

        if (!name.All(c => IsLetter(c) && ToLower(c) is >= 'a' and <= 'z'))
            errors.Add(MetadataTypeCommandErrors.InvalidName);

        if (errors.Count > 0) return errors;

        return new MetadataType(name, id);
    }
}