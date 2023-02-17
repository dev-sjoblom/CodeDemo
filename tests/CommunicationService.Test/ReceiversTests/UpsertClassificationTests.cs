using CommunicationService.Test.ClassificationTests.Helpers;
using CommunicationService.Test.ReceiversTests.Helpers;
using CommunicationService.Test.ReceiversTests.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CommunicationService.Test.ReceiversTests;

public partial class ReceiverTest
{
    private string UpsertReceiverUrl(Guid id)
    {
        return $"/ReceiverUpsert/{id}";
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, new[] { "Customer", "Partner" }, ValidMetadataTypeName,
        "DATA")]
    public async Task UpsertReceiver_NewRegistration_ShouldReturnCreated(
        string uniqueName,
        string email,
        string[] classifications,
        string metadataTypeName,
        string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddClassificationWithMetadata(classifications, metadataTypeName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertReceiverRequest(
            uniqueName,
            email,
            classifications,
            new KeyValuePair<string, string>[] { new(metadataTypeName, metadataValue) }
        ).AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertReceiverUrl(Guid.NewGuid()), body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created, stringContent);

        var responseObject = JsonConvert.DeserializeObject<ReceiverResponse>(stringContent)!;
        responseObject.Should().NotBeNull();
        responseObject.UniqueName.Should().Be(uniqueName);
        responseObject.Email.Should().Be(email);
        responseObject.Metadatas.Length.Should().Be(1);
        responseObject.Metadatas[0].Key.Should().Be(metadataTypeName);
        responseObject.Metadatas[0].Data.Should().Be(metadataValue);
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, new[] { "Customer", "Partner" }, ValidMetadataTypeName,
        "DATA")]
    public async Task UpsertReceiver_UpdateInformation_ShouldReturnNoContent(
        string uniqueName,
        string email,
        string[] classifications,
        string metadataTypeName,
        string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddClassificationWithMetadata(classifications, metadataTypeName);

        var receiver = dbContext.AddReceiverWithMetadata(
            "newUniqueName",
            "newEmail",
            new[] { "newSomeClass" },
            "newSomeMeteData",
            "newData!");
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertReceiverRequest(
            uniqueName,
            email,
            classifications,
            new KeyValuePair<string, string>[] { new(metadataTypeName, metadataValue) }
        ).AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertReceiverUrl(receiver.Id), body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent, stringContent);

        var storedReceiver = await dbContext.Receiver.SingleAsync(x => x.Id == receiver.Id);
        storedReceiver.Should().NotBeNull();
        storedReceiver.UniqueName.Should().Be(uniqueName);
        storedReceiver.Email.Should().Be(email);
        var metadata = storedReceiver.Metadatas.FirstOrDefault(x => x.MetadataType.Name == metadataTypeName)!;
        metadata.Should().NotBeNull();
        metadata.Data.Should().Be(metadataValue);
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, ValidClassificationName)]
    public async Task UpsertReceiver_UpdateWithBusyName_ShouldReturnConflict(string receiverName, string receiverEmail,
        string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        //var classification = dbContext.AddClassification(classificationName);
        dbContext.AddReceiverWithClassifications(receiverName, receiverEmail, new[] { classificationName });
        var receiver =
            dbContext.AddReceiverWithClassifications($"{receiverName}A", $"{receiverEmail}A",
                new[] { $"{classificationName}A" });

        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var url = UpsertReceiverUrl(Guid.NewGuid());
        var body = new UpsertReceiverRequest(
                receiver.UniqueName,
                receiverEmail,
                new[] { classificationName },
                Array.Empty<KeyValuePair<string, string>>())
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(url, body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict, stringContent);
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task UpsertReceiver_CreateNewWithInvalidClassificationMetadataCombination_ReturnsFailedDependency(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataType(metadataTypeName);
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();

        var url = UpsertReceiverUrl(Guid.NewGuid());
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateReceiverRequest()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await client.PutAsync(url, body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.FailedDependency, stringContent);
    }
    
    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task UpsertReceiver_UpdateReceiverWithInvalidClassificationMetadataCombination_ReturnsFailedDependency(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataType(metadataTypeName);
        dbContext.AddReceiverWithClassifications(uniqueName, email, new[] { classificationName });
        await dbContext.SaveChangesAsync();

        var url = UpsertReceiverUrl(Guid.NewGuid());
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateReceiverRequest()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await client.PutAsync(url, body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.FailedDependency, stringContent);
    }
}