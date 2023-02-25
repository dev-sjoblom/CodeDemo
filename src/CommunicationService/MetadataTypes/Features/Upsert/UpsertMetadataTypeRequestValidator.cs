using CommunicationService.MetadataTypes.DataAccess;

namespace CommunicationService.MetadataTypes.Features.Upsert;

public class UpsertMetadataTypeRequestValidator : AbstractValidator<UpsertMetadataTypeRequest>
{
    public UpsertMetadataTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .Length(
                MetadataTypeConstants.MinNameLength,
                MetadataTypeConstants.MaxNameLength)
            .Matches(MetadataTypeConstants.NameMatchRule)
            .WithMessage(MetadataTypeConstants.NamingDescription);

        RuleFor(x => x.Classifications)
            .NotNull();
    }
}