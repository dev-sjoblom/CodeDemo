using CommunicationService.Test.ReceiversTests.Fundamental;

namespace CommunicationService.Test.ReceiversTests;

public partial class ReceiverTest
{
    private string GetReceiverByIdUrl(Guid id) => $"/Receiver/{id}";

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail,
        new[] { "Customer", "Partner" },
        ValidMetadataTypeName, "DATA")]
    public async Task GetReceiverById_WithCorrectId_ReturnsCorrectReceiver(
        string uniqueName, string email,
        string[] classifications,
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var receiver =
            dbContext.AddReceiverWithMetadata(uniqueName, email, classifications, metadataTypeName, metadataValue);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var url = GetReceiverByIdUrl(receiver.Id);

        // act
        var response = await client.GetAsync(url);

        // assert
        var responseObject = await ValidateReceiverResponse(response, HttpStatusCode.OK);
        responseObject.UniqueName.Should().Be(receiver.UniqueName);
        responseObject.Email.Should().Be(receiver.Email);
        responseObject.Classifications.Length.Should().Be(classifications.Length);
        responseObject.Metadatas.Length.Should().Be(1);
        responseObject.Metadatas[0].Key.Should().Be(metadataTypeName);
        responseObject.Metadatas[0].Data.Should().Be(metadataValue);
    }
    
    [Theory]
    [AutoMoq]
    public async Task GetReceiverById_WithIncorrectId_ReturnsNotFound(Guid id)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var url = GetReceiverByIdUrl(id);
    
        // act
        var response = await client.GetAsync(url);
        
        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ReceiverEntityName));
    }
}