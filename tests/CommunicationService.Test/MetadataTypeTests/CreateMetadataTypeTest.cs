using CommunicationService.MetadataTypes.Contracts;
using CommunicationService.Test.ClassificationTests;
using CommunicationService.Test.MetadataTypeTests.Helpers;

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
    [InlineAutoMoq(ValidMetadataTypeName, ValidClassificationName)]
    [InlineAutoMoq(ValidShortestMetadataTypeName, ValidShortestClassificationName)]
    [InlineAutoMoq(ValidLongestMetadataTypeName, ValidLongestClassificationName)]
    public async Task CreateNewMetadata_WithCorrectName_ShouldStoreDataAndReturnCreated(string metadataType,
        string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateMetadataTypeRequest(
                Name: metadataType,
                Classifications: new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await client.PostAsync(CreateNewMetadataUrl(), body);
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
    [InlineAutoMoq($"{ValidMetadataTypeName}&")]
    [InlineAutoMoq($"{ValidLongestMetadataTypeName}a")]
    public async Task CreateNewMetadataType_WithIncorrectName_ReturnsBadRequest(string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateMetadataTypeRequest(
                Name: metadataTypeName,
                Classifications: Array.Empty<string>())
            .AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewMetadataUrl(), body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineAutoMoq(ValidMetadataTypeName, ValidClassificationName)]
    public async Task CreateNewMetadataType_WithNonExistingClassification_ReturnsNotFound(string metadataTypeName,
        string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateMetadataTypeRequest(
                Name: metadataTypeName,
                Classifications: new[] { classificationName })
            .AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewMetadataUrl(), body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}