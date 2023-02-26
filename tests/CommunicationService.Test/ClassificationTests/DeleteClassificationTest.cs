using CommunicationService.Test.ClassificationTests.Fundamental;

namespace CommunicationService.Test.ClassificationTests;

[Collection("Test collection")]
public class DeleteClassificationTest: IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }
    public DeleteClassificationTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }

    private string DeleteClassificationByIdUrl(Guid id) => $"/Classification/{id}";
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabaseAsync();

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task DeleteClassificationById_WithCorrectId_ReturnNoContent(string classificationName,
        string metadataTypeName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();

        var classification = dbContext.AddClassificationWithMetadata(classificationName, metadataTypeName);
        await dbContext.SaveChangesAsync();

        var url = DeleteClassificationByIdUrl(classification.Id);

        // act
        var response = await Client.DeleteAsync(url);
        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);

        var removedClassification = dbContext.Classification.FirstOrDefault(x => x.Id == classification.Id);
        removedClassification.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteClassificationById_WithInCorrectId_ReturnNotFound()
    {
        // arr
        var url = DeleteClassificationByIdUrl(Guid.NewGuid());

        // act
        var response = await Client.DeleteAsync(url);

        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ClassificationEntityName));
    }
}