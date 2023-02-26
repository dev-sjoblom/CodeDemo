namespace CommunicationService.Test.Fundamental.Wiring;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<CommunicationApiFactory>
{
}