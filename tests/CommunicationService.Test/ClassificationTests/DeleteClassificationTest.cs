using CommunicationService.Test.ClassificationTests.Fundamental;

namespace CommunicationService.Test.ClassificationTests;

public partial class ClassificationTests
{
    private string DeleteClassificationByIdUrl(Guid id) => $"/Classification/{id}";

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task DeleteClassificationById_WithCorrectId_ReturnNoContent(string classificationName,
        string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var classification = dbContext.AddClassificationWithMetadata(classificationName, metadataTypeName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var url = DeleteClassificationByIdUrl(classification.Id);

        // act
        var response = await client.DeleteAsync(url);
        var stringContent = await response.Content.ReadAsStringAsync();

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
        var url = DeleteClassificationByIdUrl(Guid.NewGuid());

        // act
        var response = await client.DeleteAsync(url);

        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ClassificationEntityName));
    }
}