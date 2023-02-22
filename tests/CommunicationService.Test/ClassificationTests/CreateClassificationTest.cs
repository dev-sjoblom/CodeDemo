using CommunicationService.Test.ClassificationTests.Helpers;
using CommunicationService.Test.ClassificationTests.Model;

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
    [InlineAutoMoq(ValidClassificationName, ValidMetadataTypeName)]
    [InlineAutoMoq(ValidShortestClassificationName, ValidShortestMetadataTypeName)]
    [InlineAutoMoq(ValidLongestClassificationName, ValidLongestMetadataTypeName)]
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
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created, stringContent);
        
        var storedClassification = dbContext.Classification.FirstOrDefault(x => x.Name == classificationName);
        storedClassification.Should().NotBeNull();
        
        var storedMetadataRelation = storedClassification?.MetadataTypes.FirstOrDefault();
        storedMetadataRelation.Should().NotBeNull();
    }

    [Theory]
    [InlineAutoMoq("a")]
    [InlineAutoMoq("a a")]
    [InlineAutoMoq($"{ValidClassificationName}&")]
    [InlineAutoMoq($"{ValidLongestClassificationName}a")]
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
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineAutoMoq(ValidClassificationName, ValidMetadataTypeName)]
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
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineAutoMoq(ValidClassificationName)]
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
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}