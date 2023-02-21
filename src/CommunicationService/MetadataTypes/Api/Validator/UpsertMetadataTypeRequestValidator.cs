using CommunicationService.MetadataTypes.Api.Model;
using CommunicationService.MetadataTypes.Data;
using FluentValidation;

namespace CommunicationService.MetadataTypes.Api.Validator;

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