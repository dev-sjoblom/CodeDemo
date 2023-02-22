using FluentValidation;

namespace CommunicationService.Classifications.Features.Upsert;

public class UpsertClassificationRequestValidator : AbstractValidator<UpsertClassificationRequest>
{
    public UpsertClassificationRequestValidator()
    {
        RuleFor(x => x.Name)
            .Length(
                ClassificationConstants.MinNameLength,
                ClassificationConstants.MaxNameLength)
            .Matches(ClassificationConstants.NameMatchRule)
            .WithMessage(ClassificationConstants.NamingDescription);

        RuleFor(x => x.MetadataTypes)
            .NotNull();
    }
}