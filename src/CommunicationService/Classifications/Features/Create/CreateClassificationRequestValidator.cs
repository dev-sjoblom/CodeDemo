using CommunicationService.Classifications.DataAccess;

namespace CommunicationService.Classifications.Features.Create;

public class CreateClassificationRequestValidator : AbstractValidator<CreateClassificationRequest>
{
    public CreateClassificationRequestValidator()
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