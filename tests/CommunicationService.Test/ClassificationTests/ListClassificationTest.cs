using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.ClassificationTests.Response;
using Newtonsoft.Json;

namespace CommunicationService.Test.ClassificationTests;

[Collection("Test collection")]
public class ListClassificationTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }
    public ListClassificationTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    
    private string ListClassificationUrl() => "/Classification";
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task ListClassification_WithData_ReturnsList(string classificationName, string metadataName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddClassificationWithMetadata(classificationName, metadataName);
        await dbContext.SaveChangesAsync();
        var url = ListClassificationUrl();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        var responseContent = await ValidateResponse(response, HttpStatusCode.OK);
        var responseObject = JsonConvert.DeserializeObject<ClassificationResponseItem[]>(
            responseContent)!;

        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        responseObject[0].Name.Should().Be(classificationName);
        responseObject[0].MetadataTypes.Length.Should().Be(1);
        responseObject[0].MetadataTypes[0].Should().Be(metadataName);
    }
}