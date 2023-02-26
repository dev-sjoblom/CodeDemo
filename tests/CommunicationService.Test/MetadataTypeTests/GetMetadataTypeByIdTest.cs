namespace CommunicationService.Test.MetadataTypeTests;

[Collection("Test collection")]
public class GetMetadataTypeByIdTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }

    public GetMetadataTypeByIdTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    private string GetMetadataTypeByIdUrl(Guid id) => $"/MetadataType/{id}";
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabaseAsync();

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task GetMetadataTypeById_WithCorrectId_ReturnsCorrectMetadataType(string metadataTypeName,
        string classificationName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        var metadataTypeItem = dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();
        var url = GetMetadataTypeByIdUrl(metadataTypeItem.Id);

        // act
        var response = await Client.GetAsync(url);

        // assert
        var responseObject = await ValidateMetadataResponse(response, 
            HttpStatusCode.OK);
        responseObject.Name.Should().Be(metadataTypeName);
        responseObject.Classifications.Length.Should().Be(1);
        responseObject.Classifications[0].Should().Be(classificationName);
    }

    [Theory]
    [AutoMoq]
    public async Task GetMetadataTypeById_WithIncorrectId_ReturnsNotFound(Guid id)
    {
        // arr
        var url = GetMetadataTypeByIdUrl(id);

        // act
        var response = await Client.GetAsync(url);

        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(MetadataTypeEntityName));
    }
}