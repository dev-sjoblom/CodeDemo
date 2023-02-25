using CommunicationService.Receivers.DataAccess;

namespace CommunicationService.Receivers.Features.Create;

public class CreateReceiverRequestValidator : AbstractValidator<CreateReceiverRequest>
{
    public CreateReceiverRequestValidator()
    {
        RuleFor(x => x.UniqueName)
            .Length(
                ReceiverConstants.MinNameLength,
                ReceiverConstants.MaxNameLength);

        RuleFor(x => x.Classifications)
            .NotNull()
            .Must(x => x.Length >= 1)
            .WithMessage("Least one classification needs to be specified");

        RuleFor(x => x.Metadata)
            .NotNull();
    }
}