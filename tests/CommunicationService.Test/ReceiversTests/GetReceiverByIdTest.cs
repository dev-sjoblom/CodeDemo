using CommunicationService.Test.ReceiversTests.Fundamental;

namespace CommunicationService.Test.ReceiversTests;

[Collection("Test collection")]
public class GetReceiverByIdTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }
    public GetReceiverByIdTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    
    private string GetReceiverByIdUrl(Guid id) => $"/Receiver/{id}";
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail,
        new[] { "Customer", "Partner" },
        ValidMetadataTypeName, "DATA")]
    public async Task GetReceiverById_WithCorrectId_ReturnsCorrectReceiver(
        string uniqueName, string email,
        string[] classifications,
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        var receiver =
            dbContext.AddReceiverWithMetadata(uniqueName, email, classifications, metadataTypeName, metadataValue);
        await dbContext.SaveChangesAsync();
        var url = GetReceiverByIdUrl(receiver.Id);

        // act
        var response = await Client.GetAsync(url);

        // assert
        var responseObject = await ValidateReceiverResponse(response, HttpStatusCode.OK);
        responseObject.UniqueName.Should().Be(receiver.UniqueName);
        responseObject.Email.Should().Be(receiver.Email);
        responseObject.Classifications.Length.Should().Be(classifications.Length);
        responseObject.Metadatas.Length.Should().Be(1);
        responseObject.Metadatas[0].Key.Should().Be(metadataTypeName);
        responseObject.Metadatas[0].Data.Should().Be(metadataValue);
    }
    
    [Theory]
    [AutoMoq]
    public async Task GetReceiverById_WithIncorrectId_ReturnsNotFound(Guid id)
    {
        // arr
        var url = GetReceiverByIdUrl(id);
        
        // act
        var response = await Client.GetAsync(url);
        
        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ReceiverEntityName));
    }
}