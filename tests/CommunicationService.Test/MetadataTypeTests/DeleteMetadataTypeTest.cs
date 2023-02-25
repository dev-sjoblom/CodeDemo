namespace CommunicationService.Test.MetadataTypeTests;
 
public partial class MetadataTypeTests
{
    private string DeleteMetadataTypeByIdUrl(Guid id) => $"/MetadataType/{id}";

    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    public async Task DeleteMetadataTypeById_WithCorrectId_ReturnNoContent(string metadataTypeName, string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var classification = dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var url = DeleteMetadataTypeByIdUrl(classification.Id);

        // act
        var response = await client.DeleteAsync(url);

        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);

        var removedClassification = dbContext.Classification.FirstOrDefault(x => x.Id == classification.Id);
        removedClassification.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteClassificationById_WithInCorrectId_ReturnNotFound()
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var url = DeleteMetadataTypeByIdUrl(Guid.NewGuid());

        // act
        var response = await client.DeleteAsync(url);
        
        // assert
        await ValidateResponseProblem(response,
            HttpStatusCode.NotFound,
            WasNotFoundTitle(MetadataTypeEntityName));
    }
}