using CommunicationService.Test.ClassificationTests.Helpers;
using CommunicationService.Test.ClassificationTests.Model;
using Newtonsoft.Json;

namespace CommunicationService.Test.ClassificationTests;

public partial class ClassificationTests
{
    private string GetClassificationByIdUrl(Guid id) => $"/Classification/{id}";
    
    [Theory]
    [InlineAutoMoq(ValidClassificationName, ValidMetadataTypeName)]
    public async Task GetClassificationById_WithCorrectId_ReturnsCorrectClassification (string classificationName, string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
        var classificationItem = dbContext.AddClassificationWithMetadata(classificationName, metadataTypeName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var url = GetClassificationByIdUrl(classificationItem.Id);

        // act
        var response = await client.GetAsync(url);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseObject = JsonConvert.DeserializeObject<ClassificationResponseItem>(jsonResponse)!;
        responseObject.Should().NotBeNull();
        responseObject.Name.Should().Be(classificationName);
        responseObject.MetadataTypes.Length.Should().Be(1);
        responseObject.MetadataTypes[0].Should().Be(metadataTypeName);
    }
    
    [Theory]
    [AutoMoq]
    public async Task GetClassificationById_WithIncorrectId_ReturnsNotFound(Guid id)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var url = GetClassificationByIdUrl(id);

        // act
        var response = await client.GetAsync(url);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound, jsonResponse);
    }
}