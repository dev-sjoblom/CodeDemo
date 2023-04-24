using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.Fundamental.Helpers;
using CommunicationService.Test.ReceiversTests.Fundamental;
using CommunicationService.Test.ReceiversTests.Model;
using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.ReceiversTests;

[Collection("Test collection")]
public class CreateReceiverTest : IAsyncLifetime
{
    private HttpClient Client { get; }
    
    private CommunicationApiFactory ApiFactory { get; }
    public CreateReceiverTest(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }

    private string CreateNewReceiverUrl()
    {
        return $"/Receiver";
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, new[] { "Customer", "Partner" }, ValidMetadataTypeName,
        "DATA")]
    public async Task CreateNewReceiver_WithCorrectData_ShouldStoreDataAndReturnCreated(
        string uniqueName,
        string email,
        string[] classifications,
        string metadataTypeName,
        string metadataValue)
    {
        // arr
        await using var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classifications);
        await dbContext.SaveChangesAsync();
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = classifications,
            Metadata = new KeyValuePair<string, string>[] { new(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // act
        var response = await Client.PostAsync(CreateNewReceiverUrl(), body);
        
        // assert
        var responseObject = await ValidateReceiverResponse(response, HttpStatusCode.Created);
        responseObject.UniqueName.Should().Be(uniqueName);
        responseObject.Email.Should().Be(email);
        responseObject.Classifications.Should().BeEquivalentTo(classifications);
        responseObject.Metadatas.Should().Contain(x => 
            x.Key == metadataTypeName && x.Data == metadataValue);

        var storedReceiver = dbContext
            .Receiver
            .Include(x => x.Classifications)
            .Include(x => x.Metadatas)
            .FirstOrDefault(x => x.UniqueName == uniqueName);
        storedReceiver.Should().NotBeNull();

        storedReceiver!.Classifications.Count.Should().Be(classifications.Length);

        storedReceiver.Metadatas.Count.Should().Be(1);
        storedReceiver.Metadatas[0].Data.Should().Be(metadataValue);
        storedReceiver.Metadatas[0].MetadataType.Name.Should().Be(metadataTypeName);
    }

    [Theory]
    [PopulateArguments("a", ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    [PopulateArguments("aa", ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task CreateNewReceiver_WithToShortName_ReturnsBadRequest(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();
        var url = CreateNewReceiverUrl();
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(url, body);

        // Assert
        await ValidateResponseProblem(response, HttpStatusCode.BadRequest,
            ValidationProblemTitle,
            "UniqueName");
    }

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail)]
    public async Task CreateNewReceiver_WithNonExistingClassification_ReturnsNotFound(
        string uniqueName, string email,
        string classificationName)
    {
        // arr
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = Array.Empty<KeyValuePair<string, string>>()
        }.AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateNewReceiverUrl(), body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.NotFound, 
            WasNotFoundTitle(ClassificationEntityName));
    }

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task CreateNewReceiver_WithInvalidClassificationMetadataCombination_FailedDependency(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataType(metadataTypeName);
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateNewReceiverUrl(), body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.FailedDependency,
            "MetadataType not allowed.");
    }

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, ValidClassificationName, ValidMetadataTypeName, "DATA")]
    public async Task CreateNewReceiver_MissingClassification_ReturnsBadRequest(
        string uniqueName, string email, string classificationName, string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        await dbContext.SaveChangesAsync();
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = Array.Empty<string>(),
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await Client.PostAsync(CreateNewReceiverUrl(), body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.BadRequest,
            ValidationProblemTitle,
            "Classifications",
            "Least one classification needs to be specified");
    }
}