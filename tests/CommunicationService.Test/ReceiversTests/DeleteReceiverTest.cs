using CommunicationService.Test.ReceiversTests.Fundamental;

namespace CommunicationService.Test.ReceiversTests;

[Collection("Test collection")]
public class DeleteReceiverTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }
    public DeleteReceiverTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    
    private string DeleteReceiverByIdUrl(Guid id) => $"/Receiver/{id}";
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabaseAsync();
    

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, 
        new[] { "Customer", "Partner" }, 
        ValidMetadataTypeName, "DATA")]
    public async Task DeleteReceiverById_WithCorrectId_ReturnNoContent(
        string uniqueName, string email,
        string[] classifications,
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        var receiver = dbContext.AddReceiverWithMetadata(uniqueName, email, classifications, metadataTypeName, metadataValue);
        await dbContext.SaveChangesAsync();
        var url = DeleteReceiverByIdUrl(receiver.Id);

        // act
        var response = await Client.DeleteAsync(url);

        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);

        var removedReceiver = dbContext.Receiver.FirstOrDefault(x => x.Id == receiver.Id);
        removedReceiver.Should().BeNull();
    }

    [Fact]
    public async Task DeleteReceiverById_WithInCorrectId_ReturnNotFound()
    {
        // arr
        var url = DeleteReceiverByIdUrl(Guid.NewGuid());

        // act
        var response = await Client.DeleteAsync(url);

        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ReceiverEntityName));
    }
}