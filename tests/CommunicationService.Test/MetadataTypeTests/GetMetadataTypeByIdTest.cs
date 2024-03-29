namespace CommunicationService.Test.MetadataTypeTests;

public partial class MetadataTypeTests
{
    private string GetMetadataTypeByIdUrl(Guid id) => $"/MetadataType/{id}";

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task GetMetadataTypeById_WithCorrectId_ReturnsCorrectMetadataType(string metadataTypeName,
        string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var metadataTypeItem = dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var url = GetMetadataTypeByIdUrl(metadataTypeItem.Id);

        // act
        var response = await client.GetAsync(url);

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
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var url = GetMetadataTypeByIdUrl(id);

        // act
        var response = await client.GetAsync(url);

        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(MetadataTypeEntityName));
    }
}