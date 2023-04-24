using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.Fundamental.Helpers;
using CommunicationService.Test.MetadataTypeTests.Fundamental;
using CommunicationService.Test.MetadataTypeTests.Model;
using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.MetadataTypeTests;

[Collection("Test collection")]
public class CreateMetadataTypeTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }

    public CreateMetadataTypeTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }

    private string CreateNewMetadataUrl() => $"/MetadataType";
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    [PopulateArguments(ValidShortestMetadataTypeName, ValidShortestClassificationName)]
    [PopulateArguments(ValidLongestMetadataTypeName, ValidLongestClassificationName)]
    public async Task CreateNewMetadata_WithCorrectName_ShouldStoreDataAndReturnCreated(
        string metadataTypeName,
        string classificationName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        var body = new CreateMetadataTypeRequestParameters(
                Name: metadataTypeName,
                Classifications: new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await Client.PostAsync(CreateNewMetadataUrl(), body);

        // assert
        var responseObject = await ValidateMetadataResponse(response, HttpStatusCode.Created);
        responseObject.Name.Should().Be(metadataTypeName);
        responseObject.Classifications.Should().Contain(classificationName);
        
        var storedClassification = await dbContext.Classification
            .Include(x => x.MetadataTypes)
            .FirstOrDefaultAsync(x => x.Name == classificationName);
        storedClassification.Should().NotBeNull();

        var storedMetadataRelation = storedClassification?.MetadataTypes.FirstOrDefault();
        storedMetadataRelation.Should().NotBeNull();
    }

    [Theory]
    [PopulateArguments("a")]
    [PopulateArguments("a a")]
    [PopulateArguments($"{ValidMetadataTypeName}&")]
    [PopulateArguments($"{ValidLongestMetadataTypeName}a")]
    public async Task CreateNewMetadataType_WithIncorrectName_ReturnsBadRequest(string metadataTypeName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        var body = new CreateMetadataTypeRequestParameters(
                Name: metadataTypeName,
                Classifications: Array.Empty<string>())
            .AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateNewMetadataUrl(), body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.BadRequest,
             ValidationProblemTitle);
    }

    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    public async Task CreateNewMetadataType_WithNonExistingClassification_ReturnsNotFound(string metadataTypeName,
        string classificationName)
    {
        // arr
        var body = new CreateMetadataTypeRequestParameters(
                Name: metadataTypeName,
                Classifications: new[] { classificationName })
            .AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateNewMetadataUrl(), body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ClassificationEntityName));
    }
}