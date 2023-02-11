using CommunicationService.MetadataTypes.Contracts;
using CommunicationService.Test.MetadataTypeTests.Helpers;
using Newtonsoft.Json;

namespace CommunicationService.Test.MetadataTypeTests;

public partial class MetadataTypeTests
{
    private string ListMetadataType() => "/MetadataType";
    [Theory]
    [InlineAutoMoq(ValidMetadataTypeName, ValidClassificationName)]
    public async Task ListMetadataTypes_WithData_ReturnsList(string metadataTypeName, string classification)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
       
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classification);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
    
        // Act
        var response = await client.GetAsync(ListMetadataType());
        var jsonResponse = await response.Content.ReadAsStringAsync();
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseObject = JsonConvert.DeserializeObject<MetadataTypeResponse[]>(jsonResponse)!;
        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        responseObject[0].Name.Should().Be(metadataTypeName);
        responseObject[0].Classifications.Length.Should().Be(1);
        responseObject[0].Classifications[0].Should().Be(classification);
    }
}