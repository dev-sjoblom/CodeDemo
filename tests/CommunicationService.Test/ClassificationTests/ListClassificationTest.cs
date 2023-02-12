using CommunicationService.Test.ClassificationTests.Helpers;
using Newtonsoft.Json;

namespace CommunicationService.Test.ClassificationTests;

public partial class ClassificationTests
{
    private string ListClassificationUrl() => "/ClassificationList";
    [Theory]
    [InlineAutoMoq(ValidClassificationName, ValidMetadataTypeName)]
    public async Task ListClassification_WithData_ReturnsList(string classificationName, string metadataName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
        dbContext.AddClassificationWithMetadata(classificationName, metadataName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
    
        // Act
        var response = await client.GetAsync(ListClassificationUrl());
        var jsonResponse = await response.Content.ReadAsStringAsync();
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseObject = JsonConvert.DeserializeObject<ClassificationResponse[]>(jsonResponse)!;
        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        responseObject[0].Name.Should().Be(classificationName);
        responseObject[0].MetadataTypes.Length.Should().Be(1);
        responseObject[0].MetadataTypes[0].Should().Be(metadataName);
    }
}