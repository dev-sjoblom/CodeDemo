using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.Fundamental.Helpers;
using CommunicationService.Test.ReceiversTests.Fundamental;
using CommunicationService.Test.ReceiversTests.Model;
using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.ReceiversTests;

[Collection("Test collection")]
public class UpsertReceiverTests : IAsyncLifetime
{
    private HttpClient Client { get; }
    
    private CommunicationApiFactory ApiFactory { get; }
    public UpsertReceiverTests(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    
    private string UpsertReceiverUrl(Guid id)
    {
        return $"/Receiver/{id}";
    }
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, new[] { "Customer", "Partner" }, ValidMetadataTypeName,
        "DATA")]
    public async Task UpsertReceiver_NewRegistration_ShouldReturnCreated(
        string uniqueName,
        string email,
        string[] classifications,
        string metadataTypeName,
        string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddClassificationWithMetadata(classifications, metadataTypeName);
        await dbContext.SaveChangesAsync();
        var url = UpsertReceiverUrl(Guid.NewGuid());
        var body = new UpsertReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = classifications,
            Metadata = new KeyValuePair<string, string>[] { new(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // act
        var response = await Client.PutAsync(url, body);

        // assert
        var responseObject = await ValidateReceiverResponse(response, 
            HttpStatusCode.Created);

        responseObject.Should().NotBeNull();
        responseObject.UniqueName.Should().Be(uniqueName);
        responseObject.Email.Should().Be(email);
        responseObject.Metadatas.Length.Should().Be(1);
        responseObject.Metadatas[0].Key.Should().Be(metadataTypeName);
        responseObject.Metadatas[0].Data.Should().Be(metadataValue);
    }

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, 
        new[] { "Customer", "Partner" }, 
        ValidMetadataTypeName, "DATA")]
    public async Task UpsertReceiver_UpdateInformation_ShouldReturnNoContent(
        string uniqueName, string email,
        string[] classifications,
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddClassificationWithMetadata(classifications, metadataTypeName);
        var receiver = dbContext.AddReceiverWithMetadata(
            $"A{uniqueName}",
            $"A{email}",
            new[] { $"A{classifications[0]}"},
            $"A{metadataTypeName}",
            $"A{metadataValue}");
        await dbContext.SaveChangesAsync();
        var url = UpsertReceiverUrl(receiver.Id);
        var body = new UpsertReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = classifications,
            Metadata = new KeyValuePair<string, string>[] { new(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // act
        var response = await Client.PutAsync(url, body);

        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);
        await dbContext.Entry(receiver).ReloadAsync();
        var storedReceiver = await dbContext.Receiver
            .Include(x => x.Metadatas)
            .SingleAsync(x => x.Id == receiver.Id);
        
        storedReceiver.Should().NotBeNull();
        storedReceiver.UniqueName.Should().Be(uniqueName);
        storedReceiver.Email.Should().Be(email);
        
        var metadata = storedReceiver.Metadatas.FirstOrDefault(x => x.MetadataType.Name == metadataTypeName)!;
        metadata.Should().NotBeNull();
        metadata.Data.Should().Be(metadataValue);
    }

    [Theory]
    [PopulateArguments(
        ValidReceiverName, ValidReceiverEmail, 
        $"other{ValidReceiverName}", $"other{ValidReceiverEmail}", 
        ValidClassificationName)]
    public async Task UpsertReceiver_UpdateWithBusyName_ShouldReturnConflict(
        string name, string email,
        string otherName, string otherEmail,
        string classificationName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddReceiverWithClassifications(name, email, new[] { classificationName });
        var receiver =
            dbContext.AddReceiverWithClassifications(otherName, otherEmail,
                new[] { classificationName });
        await dbContext.SaveChangesAsync();
        var url = UpsertReceiverUrl(Guid.NewGuid());
        var body = new UpsertReceiverRequestParameters()
            {
                UniqueName = receiver.UniqueName,
                Email = email,
                Classifications = new[] { classificationName },
                Metadata = Array.Empty<KeyValuePair<string, string>>()
            }
            .AsJsonStringContent();

        // act
        var response = await Client.PutAsync(url, body);

        // assert
        response.StatusCode.Should().Be(
            HttpStatusCode.Conflict, 
            "Receiver name already taken.");
    }

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, 
        ValidClassificationName, 
        ValidMetadataTypeName, "DATA")]
    public async Task UpsertReceiver_CreateNewWithInvalidClassificationMetadataCombination_ReturnsFailedDependency(
        string uniqueName, string email, 
        string classificationName, 
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataType(metadataTypeName);
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        var url = UpsertReceiverUrl(Guid.NewGuid());
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await Client.PutAsync(url, body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.FailedDependency, 
            "MetadataType not allowed.");
    }

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, 
        ValidClassificationName, 
        ValidMetadataTypeName, "DATA")]
    public async Task UpsertReceiver_UpdateReceiverWithInvalidClassificationMetadataCombination_ReturnsFailedDependency(
        string uniqueName, string email, 
        string classificationName, 
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataType(metadataTypeName);
        dbContext.AddReceiverWithClassifications(uniqueName, email, new[] { classificationName });
        await dbContext.SaveChangesAsync();
        var url = UpsertReceiverUrl(Guid.NewGuid());
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classificationName },
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await Client.PutAsync(url, body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.FailedDependency, 
            "MetadataType not allowed.");
    }

    [Theory]
    [PopulateArguments(ValidReceiverName, ValidReceiverEmail, 
        ValidClassificationName, 
        ValidMetadataTypeName, "DATA")]
    public async Task UpsertReceiver_MissingClassification_ReturnsBadRequest(
        string uniqueName, string email, 
        string classificationName, 
        string metadataTypeName, string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataTypeWithClassification(metadataTypeName, classificationName);
        var receiver = dbContext.AddReceiverWithClassifications(uniqueName, email, new[] { classificationName });
        await dbContext.SaveChangesAsync();
        var url = UpsertReceiverUrl(receiver.Id);
        var body = new UpsertReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = Array.Empty<string>(),
            Metadata = new[] { new KeyValuePair<string, string>(metadataTypeName, metadataValue) }
        }.AsJsonStringContent();

        // Act
        var response = await Client.PutAsync(url, body);

        // Assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.BadRequest, 
            withTitle: ValidationProblemTitle,
            withErrorResponseMessage: "Least one classification needs to be specified");
    }


    [Theory]
    [PopulateArguments(
        ValidReceiverName, ValidReceiverEmail,
        $"Other{ValidClassificationName}",
        $"Other{ValidMetadataTypeName}",
        ValidClassificationName,
        ValidMetadataTypeName,
        "DATA")]
    public async Task UpsertReceiver_RemoveRequiredClassification_ReturnsFailedDependency(
        string uniqueName, string email,
        string otherClassification,
        string otherMetadataType,
        string classification,
        string metadataType,
        string metadataValue)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataTypeWithClassification(metadataType, classification);
        dbContext.AddMetadataTypeWithClassification(otherMetadataType, otherClassification);
        dbContext.AddReceiverWithClassifications(uniqueName, email,
            new[] { classification, otherClassification });
        await dbContext.SaveChangesAsync();
        var url = UpsertReceiverUrl(Guid.NewGuid());
        var body = new CreateReceiverRequestParameters()
        {
            UniqueName = uniqueName,
            Email = email,
            Classifications = new[] { classification },
            Metadata = new[]
            {
                new KeyValuePair<string, string>(metadataType, metadataValue),
                new KeyValuePair<string, string>(otherMetadataType, metadataValue)
            }
        }.AsJsonStringContent();

        // Act
        var response = await Client.PutAsync(url, body);

        // Assert
        await ValidateResponseProblem(response,
            HttpStatusCode.FailedDependency,
            withTitle: $"{MetadataTypeEntityName} not allowed.");
    }
}