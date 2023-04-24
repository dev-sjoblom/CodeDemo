namespace CommunicationService.Test.MetadataTypeTests;
 
[Collection("Test collection")]
public class DeleteMetadataTypeTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }

    public DeleteMetadataTypeTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    
    private string DeleteMetadataTypeByIdUrl(Guid id) => $"/MetadataType/{id}";
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    public async Task DeleteMetadataTypeById_WithCorrectId_ReturnNoContent(string metadataTypeName, string classificationName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        var classification = dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();
        var url = DeleteMetadataTypeByIdUrl(classification.Id);

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
        var url = DeleteMetadataTypeByIdUrl(Guid.NewGuid());

        // act
        var response = await Client.DeleteAsync(url);
        
        // assert
        await ValidateResponseProblem(response,
            HttpStatusCode.NotFound,
            WasNotFoundTitle(MetadataTypeEntityName));
    }
}