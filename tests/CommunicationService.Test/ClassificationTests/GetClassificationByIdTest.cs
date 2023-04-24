using CommunicationService.Test.ClassificationTests.Fundamental;

namespace CommunicationService.Test.ClassificationTests;

[Collection("Test collection")]
public class GetClassificationByIdTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }
    public GetClassificationByIdTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }    
    private string GetClassificationByIdUrl(Guid id) => $"/Classification/{id}";
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task GetClassificationById_WithCorrectId_ReturnsCorrectClassification(string classificationName,
        string metadataTypeName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();

        var classificationItem = dbContext.AddClassificationWithMetadata(classificationName, metadataTypeName);
        await dbContext.SaveChangesAsync();

        var url = GetClassificationByIdUrl(classificationItem.Id);

        // act
        var response = await Client.GetAsync(url);

        // assert
        var responseObject = await ValidateClassificationResponse(response,
            HttpStatusCode.OK);

        responseObject.Name.Should().Be(classificationName);
        responseObject.MetadataTypes.Length.Should().Be(1);
        responseObject.MetadataTypes[0].Should().Be(metadataTypeName);
    }

    [Theory]
    [AutoMoq]
    public async Task GetClassificationById_WithIncorrectId_ReturnsNotFound(Guid id)
    {
        // arr
        var url = GetClassificationByIdUrl(id);

        // act
        var response = await Client.GetAsync(url);

        // assert
        await ValidateResponseProblem(response,
            HttpStatusCode.NotFound,
            WasNotFoundTitle(ClassificationEntityName));
    }
}