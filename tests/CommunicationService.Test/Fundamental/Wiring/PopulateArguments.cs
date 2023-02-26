using AutoFixture.Xunit2;

namespace CommunicationService.Test.Fundamental.Wiring;

public class PopulateArguments : InlineAutoDataAttribute
{
    public PopulateArguments(params object[] objects) : base(new AutoMoq(), objects)
    {
    }
}
