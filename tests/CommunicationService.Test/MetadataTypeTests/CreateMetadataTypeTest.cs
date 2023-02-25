using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.Fundamental.Helpers;
using CommunicationService.Test.MetadataTypeTests.Fundamental;
using CommunicationService.Test.MetadataTypeTests.Model;

namespace CommunicationService.Test.MetadataTypeTests;

public partial class MetadataTypeTests : IClassFixture<MetadataTypeFixture>
{
    private MetadataTypeFixture Fixture { get; }

    public MetadataTypeTests(MetadataTypeFixture fixture)
    {
        Fixture = fixture;
    }

    private string CreateNewMetadataUrl() => $"/MetadataType";

    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    [PopulateArguments(ValidShortestMetadataTypeName, ValidShortestClassificationName)]
    [PopulateArguments(ValidLongestMetadataTypeName, ValidLongestClassificationName)]
    public async Task CreateNewMetadata_WithCorrectName_ShouldStoreDataAndReturnCreated(
        string metadataTypeName,
        string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateMetadataTypeRequestParameters(
                Name: metadataTypeName,
                Classifications: new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await client.PostAsync(CreateNewMetadataUrl(), body);

        // assert
        var responseObject = await ValidateMetadataResponse(response, HttpStatusCode.Created);
        responseObject.Name.Should().Be(metadataTypeName);
        responseObject.Classifications.Should().Contain(classificationName);
        
        var storedClassification = dbContext.Classification.FirstOrDefault(x => x.Name == classificationName);
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
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateMetadataTypeRequestParameters(
                Name: metadataTypeName,
                Classifications: Array.Empty<string>())
            .AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewMetadataUrl(), body);

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
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateMetadataTypeRequestParameters(
                Name: metadataTypeName,
                Classifications: new[] { classificationName })
            .AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewMetadataUrl(), body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ClassificationEntityName));
    }
}