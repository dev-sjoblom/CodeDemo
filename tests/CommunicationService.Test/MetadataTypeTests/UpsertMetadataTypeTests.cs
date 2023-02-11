using CommunicationService.Test.ClassificationTests;
using CommunicationService.Test.MetadataTypeTests.ContractModels;
using CommunicationService.Test.MetadataTypeTests.Helpers;
using Newtonsoft.Json;

namespace CommunicationService.Test.MetadataTypeTests;

public partial class MetadataTypeTests
{
    private string UpsertMetadataTypeUrl(Guid id) => $"/MetadataType/{id}";

    [Theory]
    [InlineAutoMoq(ValidClassificationName, ValidMetadataTypeName)]
    public async Task UpsertMetadataType_NewRegistration_ShouldReturnCreated(string metadataTypeName, string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertMetadataTypeRequest(
                Name: metadataTypeName, 
                Classifications:new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertMetadataTypeUrl(Guid.NewGuid()), body);
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created, stringContent);
        
        var responseObject = JsonConvert.DeserializeObject<MetadataTypeResponse>(stringContent)!;
        responseObject.Should().NotBeNull();
        responseObject.Name.Should().Be(metadataTypeName);
        responseObject.Classifications.Length.Should().Be(1);
        responseObject.Classifications[0].Should().Be(classificationName);
    }
       
    [Theory]
    [InlineAutoMoq(ValidMetadataTypeName, ValidClassificationName)]
    public async Task UpsertMetadataType_UpdateInformation_ShouldReturnNoContent(string metadataTypeName, string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var changeToClassification = dbContext.AddClassification(classificationName);
        var metadataType = dbContext.AddMetadataTypeWithClassification("someMetadataType", "someClassification");
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertMetadataTypeRequest(
                Name: metadataTypeName, 
                Classifications:new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertMetadataTypeUrl(metadataType.Id), body);
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent, stringContent);
        
        var storedMetadataType = dbContext.MetadataType.FirstOrDefault(x => x.Id == metadataType.Id);
        storedMetadataType.Should().NotBeNull();
        storedMetadataType?.Name.Should().Be(metadataTypeName);
        
        var storedClassificationRelation = storedMetadataType?.Classifications.FirstOrDefault(x => x.Id == changeToClassification.Id);
        storedClassificationRelation.Should().NotBeNull();
    }
    
    [Theory]
    [InlineAutoMoq(ValidMetadataTypeName)]
    public async Task UpsertMetadataType_UpdateWithBusyName_ShouldReturnConflict(string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataType(metadataTypeName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertMetadataTypeRequest(
                Name: metadataTypeName, 
                Classifications: Array.Empty<string>())
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertMetadataTypeUrl(Guid.NewGuid()), body);
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict, stringContent);
    }
}