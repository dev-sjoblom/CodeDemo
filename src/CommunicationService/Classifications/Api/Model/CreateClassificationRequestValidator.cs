using CommunicationService.Classifications.Data;
using FluentValidation;

namespace CommunicationService.Classifications.Api.Model;

public class CreateClassificationRequestValidator : AbstractValidator<CreateClassificationRequest>
{
    public CreateClassificationRequestValidator()
    {
        RuleFor(x => x.Name).Length(
            ClassificationConstants.MinNameLength,
            ClassificationConstants.MaxNameLength);
        RuleFor(x => x.MetadataTypes).NotNull();
    }
}