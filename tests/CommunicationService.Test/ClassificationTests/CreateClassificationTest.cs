using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.ClassificationTests.Requests;
using CommunicationService.Test.Fundamental.Helpers;

namespace CommunicationService.Test.ClassificationTests;

public partial class ClassificationTests : IClassFixture<ClassificationFixture>
{
    private ClassificationFixture Fixture { get; set; }
    public ClassificationTests(ClassificationFixture fixture)
    {
        Fixture = fixture;
    }

    private string CreateClassificationUrl() => $"/Classification";
    
    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    [PopulateArguments(ValidShortestClassificationName, ValidShortestMetadataTypeName)]
    [PopulateArguments(ValidLongestClassificationName, ValidLongestMetadataTypeName)]
    public async Task CreateNewClassification_WithCorrectName_ShouldStoreDataAndReturnCreated(string classificationName, string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var url = CreateClassificationUrl();
        dbContext.AddMetadataType(metadataTypeName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                 MetadataTypes:new[] { metadataTypeName })
            .AsJsonStringContent();

        // act
        var response = await client.PostAsync(url, body);
        
        // assert
        await ValidateClassificationResponse(response,
            HttpStatusCode.Created);
        
        var storedClassification = dbContext.Classification.FirstOrDefault(x => x.Name == classificationName);
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
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes: Array.Empty<string>())
            .AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateClassificationUrl(), body);
        
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
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes:new[] { metadataTypeName })
            .AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateClassificationUrl(), body);
        
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
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes: Array.Empty<string>())
            .AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateClassificationUrl(), body);
        
        // Assert
        await ValidateResponseProblem(response,
            HttpStatusCode.Conflict,
            "Classification name already taken.");
    }
}