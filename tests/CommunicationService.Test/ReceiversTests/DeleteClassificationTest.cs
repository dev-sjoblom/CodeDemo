using CommunicationService.Test.ReceiversTests.Fundamental;

namespace CommunicationService.Test.ReceiversTests;

public partial class ReceiverTest
{
    private string DeleteReceiverByIdUrl(Guid id) => $"/Receiver/{id}";

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, 
        new[] { "Customer", "Partner" }, 
        ValidMetadataTypeName, "DATA")]
    public async Task DeleteReceiverById_WithCorrectId_ReturnNoContent(
        string uniqueName, string email,
        string[] classifications,
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var receiver = dbContext.AddReceiverWithMetadata(uniqueName, email, classifications, metadataTypeName, metadataValue);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var url = DeleteReceiverByIdUrl(receiver.Id);

        // act
        var response = await client.DeleteAsync(url);

        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);

        var removedReceiver = dbContext.Receiver.FirstOrDefault(x => x.Id == receiver.Id);
        removedReceiver.Should().BeNull();
    }

    [Fact]
    public async Task DeleteReceiverById_WithInCorrectId_ReturnNotFound()
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var url = DeleteReceiverByIdUrl(Guid.NewGuid());

        // act
        var response = await client.DeleteAsync(url);

        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ReceiverEntityName));
    }
}