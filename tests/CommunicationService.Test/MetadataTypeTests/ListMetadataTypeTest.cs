using CommunicationService.Test.MetadataTypeTests.Response;
using Newtonsoft.Json;

namespace CommunicationService.Test.MetadataTypeTests;

[Collection("Test collection")]
public class ListMetadataTypeTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    
    private CommunicationApiFactory ApiFactory { get; }

    public ListMetadataTypeTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    
    private string GetMetadataType() => "/MetadataType";
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabaseAsync();
    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    public async Task ListMetadataTypes_WithData_ReturnsList(string metadataTypeName, string classification)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classification);
        await dbContext.SaveChangesAsync();
        
        // Act
        var response = await Client.GetAsync(GetMetadataType());
    
        // Assert
        var responseContent = await ValidateResponse(response, HttpStatusCode.OK);
        var responseObject = JsonConvert.DeserializeObject<MetadataTypeResponseItem[]>(responseContent)!;
        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        responseObject[0].Name.Should().Be(metadataTypeName);
        responseObject[0].Classifications.Length.Should().Be(1);
        responseObject[0].Classifications[0].Should().Be(classification);
    }
}