using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using AutoFixture.Xunit2;

namespace CommunicationService.Test.Fundamental;

public class AutoMoq : AutoDataAttribute
{
    public AutoMoq() : base(() => new Fixture().Customize(new AutoFakeItEasyCustomization()))
    {
    }
}