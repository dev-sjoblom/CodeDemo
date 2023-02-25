using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.ClassificationTests.Response;
using Newtonsoft.Json;

namespace CommunicationService.Test.ClassificationTests;

public partial class ClassificationTests
{
    private string ListClassificationUrl() => "/Classification";

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task ListClassification_WithData_ReturnsList(string classificationName, string metadataName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddClassificationWithMetadata(classificationName, metadataName);
        await dbContext.SaveChangesAsync();
        var url = ListClassificationUrl();
        var client = Fixture.GetMockedClient(dbContext);

        // Act
        var response = await client.GetAsync(url);

        // Assert
        var responseContent = await ValidateResponse(response, HttpStatusCode.OK);
        var responseObject = JsonConvert.DeserializeObject<ClassificationResponseItem[]>(
            responseContent)!;

        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        responseObject[0].Name.Should().Be(classificationName);
        responseObject[0].MetadataTypes.Length.Should().Be(1);
        responseObject[0].MetadataTypes[0].Should().Be(metadataName);
    }
}