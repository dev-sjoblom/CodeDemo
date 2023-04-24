using System.Data.Common;
using CommunicationService.Classifications.DataAccess;
using CommunicationService.MetadataTypes.DataAccess;
using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.Fundamental.Helpers;
using CommunicationService.Test.MetadataTypeTests.Model;
using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Test.MetadataTypeTests;

[Collection("Test collection")]
public class UpsertMetadataTypeTests : IAsyncLifetime
{
    private HttpClient Client { get; }
    private CommunicationApiFactory ApiFactory { get; }

    public UpsertMetadataTypeTests(CommunicationApiFactory apiFactory)
    {
        ApiFactory = apiFactory;
        Client = ApiFactory.HttpClient;
    }
    private string UpsertMetadataTypeUrl(Guid id) => $"/MetadataType/{id}";
    
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => ApiFactory.ResetDatabase();

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task UpsertMetadataType_NewRegistration_ShouldReturnCreated(string metadataTypeName, string classificationName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        var body = new UpsertMetadataTypeRequestParameters(
                Name: metadataTypeName, 
                Classifications:new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await Client.PutAsync(UpsertMetadataTypeUrl(Guid.NewGuid()), body);
        
        // assert
        var responseObject = await ValidateMetadataResponse(response, HttpStatusCode.Created);
        responseObject.Should().NotBeNull();
        responseObject.Name.Should().Be(metadataTypeName);
        responseObject.Classifications.Length.Should().Be(1);
        responseObject.Classifications[0].Should().Be(classificationName);
    }
       
    [Theory]
    [PopulateArguments(ValidMetadataTypeName, ValidClassificationName)]
    public async Task UpsertMetadataType_UpdateInformation_ShouldReturnNoContent(string metadataTypeName, string classificationName)
    {
        // arr
        using var dbContext = ApiFactory.CreateDbContext();
        var changeToClassification = dbContext.AddClassification(classificationName);
        var metadataType = dbContext.AddMetadataTypeWithClassification("someMetadataType", "someClassification");
        await dbContext.SaveChangesAsync();

        var url = UpsertMetadataTypeUrl(metadataType.Id);
        var body = new UpsertMetadataTypeRequestParameters(
                Name: metadataTypeName, 
                Classifications:new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await Client.PutAsync(url, body);

        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);
        await dbContext.Entry(metadataType).ReloadAsync();
        
        var storedMetadataType = await dbContext.MetadataType
            .Include(x => x.Classifications)
            .FirstOrDefaultAsync(x => x.Id == metadataType.Id);
        storedMetadataType.Should().NotBeNull();
        storedMetadataType?.Name.Should().Be(metadataTypeName);

        var storedClassificationRelation =
            storedMetadataType?.Classifications.FirstOrDefault(x => x.Id == changeToClassification.Id);
        storedClassificationRelation.Should().NotBeNull();
    }
    
    [Theory]
    [PopulateArguments(ValidMetadataTypeName)]
    public async Task UpsertMetadataType_UpdateWithBusyName_ShouldReturnConflict(string metadataTypeName)
    {
        // arr
        var dbContext = ApiFactory.CreateDbContext();
        dbContext.AddMetadataType(metadataTypeName);
        await dbContext.SaveChangesAsync();
        var url = UpsertMetadataTypeUrl(Guid.NewGuid());
        var body = new UpsertMetadataTypeRequestParameters(
                Name: metadataTypeName, 
                Classifications: Array.Empty<string>())
            .AsJsonStringContent();

        // act
        var response = await Client.PutAsync(url, body);
        
        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.Conflict, 
            "MetadataType name already taken.");
    }
}