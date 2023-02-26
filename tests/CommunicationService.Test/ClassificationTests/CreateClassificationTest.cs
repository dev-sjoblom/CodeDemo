using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.ClassificationTests.Requests;
using CommunicationService.Test.Fundamental.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.ClassificationTests;

[Collection("Test collection")]
public class CreateClassificationTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }
    public CreateClassificationTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }

    private string CreateClassificationUrl() => $"/Classification";
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabaseAsync();
    
    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    [PopulateArguments(ValidShortestClassificationName, ValidShortestMetadataTypeName)]
    [PopulateArguments(ValidLongestClassificationName, ValidLongestMetadataTypeName)]
    public async Task CreateNewClassification_WithCorrectName_ShouldStoreDataAndReturnCreated(string classificationName, string metadataTypeName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        var url = CreateClassificationUrl();
        dbContext.AddMetadataType(metadataTypeName);
        await dbContext.SaveChangesAsync();
        
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                 MetadataTypes:new[] { metadataTypeName })
            .AsJsonStringContent();

        // act
        var response = await Client.PostAsync(url, body);
        
        // assert
        await ValidateClassificationResponse(response,
            HttpStatusCode.Created);
        
        var storedClassification = await dbContext.Classification
            .Include(x => x.MetadataTypes)
            .Where(x => x.Name == classificationName)
            .SingleOrDefaultAsync();
        storedClassification.Should().NotBeNull();
        
        var storedMetadataRelation = storedClassification?.MetadataTypes.FirstOrDefault();
        storedMetadataRelation.Should().NotBeNull();
    }

    [Theory]
    [PopulateArguments("a")]
    [PopulateArguments("a a")]
    [PopulateArguments($"{ValidClassificationName}&")]
    [PopulateArguments($"{ValidLongestClassificationName}a")]
    public async Task CreateNewClassification_WithIncorrectName_ReturnsBadRequest(string classificationName)
    {
        // arr
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes: Array.Empty<string>())
            .AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateClassificationUrl(), body);
        
        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.BadRequest, 
            ValidationProblemTitle,
            "Name");
    }
    
    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task CreateNewClassification_WithNonExistingMetadataType_ReturnsNotFound(string classificationName, string metadataTypeName)
    {
        // arr
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes:new[] { metadataTypeName })
            .AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateClassificationUrl(), body);
        
        // Assert
        await ValidateResponseProblem(response,
            HttpStatusCode.NotFound,
            WasNotFoundTitle(MetadataTypeEntityName));
    }
    
    [Theory]
    [PopulateArguments(ValidClassificationName)]
    public async Task CreateNewClassification_WithNameTaken_ReturnsBadRequest(string classificationName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes: Array.Empty<string>())
            .AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateClassificationUrl(), body);
        
        // Assert
        await ValidateResponseProblem(response,
            HttpStatusCode.Conflict,
            "Classification name already taken.");
    }
}