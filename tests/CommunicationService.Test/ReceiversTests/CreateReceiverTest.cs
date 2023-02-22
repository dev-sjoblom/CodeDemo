using CommunicationService.Test.ClassificationTests.Helpers;
using CommunicationService.Test.ReceiversTests.Helpers;
using CommunicationService.Test.ReceiversTests.Model;

namespace CommunicationService.Test.ReceiversTests;

public partial class ReceiverTest : IClassFixture<ReceiverFixture>
{
    private ReceiverFixture Fixture { get; }

    public ReceiverTest(ReceiverFixture fixture)
    {
        Fixture = fixture;
    }

    private string CreateNewReceiverUrl()
    {
        return $"/Receiver";
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, new[] { "Customer", "Partner" }, ValidMetadataTypeName,
        "DATA")]
    public async Task CreateNewReceiver_WithCorrectData_ShouldStoreDataAndReturnCreated(
        string uniqueName,
        string email,
        string[] classifications,
        string metadataTypeName,
        string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classifications);
        await dbContext.SaveChangesAsync();


        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = classifications,
            Metadata = new KeyValuePair<string, string>[] { new(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // act
        var response = await client.PostAsync(CreateNewReceiverUrl(), body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created, stringContent);

        var storedReceiver = dbContext.Receiver.FirstOrDefault(x => x.UniqueName == uniqueName);
        storedReceiver.Should().NotBeNull();

        storedReceiver!.Classifications.Count.Should().Be(classifications.Length);

        storedReceiver.Metadatas.Count.Should().Be(1);
        storedReceiver.Metadatas[0].Data.Should().Be(metadataValue);
        storedReceiver.Metadatas[0].MetadataType.Name.Should().Be(metadataTypeName);
    }

    [Theory]
    [InlineAutoMoq("a", ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    [InlineAutoMoq("aa", ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task CreateNewReceiver_WithToShortName_ReturnsBadRequest(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewReceiverUrl(), body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, stringContent);
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail)]
    public async Task CreateNewReceiver_WithNonExistingClassification_ReturnsNotFound(
        string uniqueName, string email,
        string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = Array.Empty<KeyValuePair<string, string>>()
        }.AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewReceiverUrl(), body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound, stringContent);
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task CreateNewReceiver_WithInvalidClassificationMetadataCombination_FailedDependency(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataType(metadataTypeName);
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewReceiverUrl(), body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.FailedDependency, stringContent);
    }

    [Theory]
    [InlineAutoMoq(ValidReceiverName, ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task CreateNewReceiver_MissingClassification_ReturnsBadRequest(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = Array.Empty<string>(),
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await client.PostAsync(CreateNewReceiverUrl(), body);
        var stringContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, stringContent);
        stringContent.Should().Contain("Least one classification needs to be specified");
    }
}