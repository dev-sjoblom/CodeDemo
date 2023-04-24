using CommunicationService.Test.ReceiversTests.Fundamental;
using CommunicationService.Test.ReceiversTests.Response;
using Newtonsoft.Json;

namespace CommunicationService.Test.ReceiversTests;

[Collection("Test collection")]
public class ListReceiverTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }
    public ListReceiverTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    
    private string ListMetadataType() => "/Receiver";
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail,
        new[] { "Customer", "Partner" },
        ValidMetadataTypeName, "DATA")]
    public async Task ListReceiver_WithData_ReturnsList(
            string uniqueName, string email,
            string[] classifications,
            string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        var receiver =
            dbContext.AddReceiverWithMetadata(uniqueName, email, classifications, metadataTypeName, metadataValue);
        await dbContext.SaveChangesAsync();
        var url = ListMetadataType();
    
        // Act
        var response = await Client.GetAsync(url);
    
        // Assert
        var responseContent = await ValidateResponse(response, HttpStatusCode.OK);
        var responseObject = JsonConvert.DeserializeObject<ReceiverResponseItem[]>(responseContent)!;
        
        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        
        responseObject[0].UniqueName.Should().Be(receiver.UniqueName);
        responseObject[0].Email.Should().Be(receiver.Email);
        responseObject[0].Classifications.Length.Should().Be(classifications.Length);
        responseObject[0].Metadatas.Length.Should().Be(1);
        responseObject[0].Metadatas[0].Key.Should().Be(metadataTypeName);
        responseObject[0].Metadatas[0].Data.Should().Be(metadataValue);
    }
}