using AutoFixture.Xunit2;

namespace CommunicationService.Test.Fundamental;

public class InlineAutoMoq : InlineAutoDataAttribute
{
    public InlineAutoMoq(params object[] objects) : base(new AutoMoq(), objects)
    {
    }
}
