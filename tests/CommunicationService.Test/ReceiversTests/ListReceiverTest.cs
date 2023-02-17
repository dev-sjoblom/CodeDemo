using CommunicationService.Test.ReceiversTests.Helpers;
using CommunicationService.Test.ReceiversTests.Model;
using Newtonsoft.Json;

namespace CommunicationService.Test.ReceiversTests;

public partial class ReceiverTest
{
    private string ListMetadataType() => "/ReceiverList";

    [Theory]
    [InlineAutoMoq(
        ValidReceiverName,
        ValidReceiverEmail,
        new[] { "Customer", "Partner" },
        ValidMetadataTypeName,
        "DATA")]
    public async Task ListReceiver_WithData_ReturnsList(
            string uniqueName,
            string email,
            string[] classifications,
            string metadataTypeName,
            string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
       
        var receiver =
            dbContext.AddReceiverWithMetadata(uniqueName, email, classifications, metadataTypeName, metadataValue);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
    
        // Act
        var response = await client.GetAsync(ListMetadataType());
        var jsonResponse = await response.Content.ReadAsStringAsync();
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseObject = JsonConvert.DeserializeObject<ReceiverResponse[]>(jsonResponse)!;
        
        responseObject.Should().NotBeNull();
        responseObject.Length.Should().Be(1);
        
        responseObject[0].UniqueName.Should().Be(receiver.UniqueName);
        responseObject[0].Email.Should().Be(receiver.Email);
        responseObject[0].Classifications.Length.Should().Be(classifications.Length);
        responseObject[0].Metadatas.Length.Should().Be(1);
        responseObject[0].Metadatas[0].Key.Should().Be(metadataTypeName);
        responseObject[0].Metadatas[0].Data.Should().Be(metadataValue);
    }
}