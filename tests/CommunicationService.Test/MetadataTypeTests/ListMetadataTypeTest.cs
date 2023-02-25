using CommunicationService.Test.MetadataTypeTests.Response;
using Newtonsoft.Json;

namespace CommunicationService.Test.MetadataTypeTests;

public partial class MetadataTypeTests
{
    private string GetMetadataType() => "/MetadataType";
    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    public async Task ListMetadataTypes_WithData_ReturnsList(string metadataTypeName, string classification)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
       
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classification);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
    
        // Act
        var response = await client.GetAsync(GetMetadataType());
    
        // Assert
        var responseContent = await ValidateResponse(response, HttpStatusCode.OK);
        var responseObject = JsonConvert.DeserializeObject<MetadataTypeResponseItem[]>(responseContent)!;
        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        responseObject[0].Name.Should().Be(metadataTypeName);
        responseObject[0].Classifications.Length.Should().Be(1);
        responseObject[0].Classifications[0].Should().Be(classification);
    }
}