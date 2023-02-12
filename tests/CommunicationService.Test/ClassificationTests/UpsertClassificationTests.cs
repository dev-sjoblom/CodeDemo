using CommunicationService.Test.ClassificationTests.Helpers;
using Newtonsoft.Json;

namespace CommunicationService.Test.ClassificationTests;

public partial class ClassificationTests
{
    private string UpsertClassificationUrl(Guid id) => $"/ClassificationUpsert/{id}";

    [Theory]
    [InlineAutoMoq(ValidClassificationName, ValidMetadataTypeName)]
    public async Task UpsertClassification_NewRegistration_ShouldReturnCreated(string classificationName, string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataType(metadataTypeName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertClassificationRequest(
                Name: classificationName, 
                MetadataTypes:new[] { metadataTypeName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertClassificationUrl(Guid.NewGuid()), body);
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert   at FluentAssertions.Execution.XUnit2TestFramework.Throw(String message)

        response.StatusCode.Should().Be(HttpStatusCode.Created, stringContent);
        
        var responseObject = JsonConvert.DeserializeObject<ClassificationResponse>(stringContent)!;
        responseObject.Should().NotBeNull();
        responseObject.Name.Should().Be(classificationName);
        responseObject.MetadataTypes.Length.Should().Be(1);
        responseObject.MetadataTypes[0].Should().Be(metadataTypeName);
    }
       
    [Theory]
    [InlineAutoMoq(ValidClassificationName, ValidMetadataTypeName)]
    public async Task UpsertClassification_UpdateInformation_ShouldReturnNoContent(string classificationName, string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddMetadataType(metadataTypeName);
        var classification = dbContext.AddClassificationWithMetadata("someClassification", "someMetadataType");
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertClassificationRequest(
                Name: classificationName, 
                MetadataTypes:new[] { metadataTypeName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertClassificationUrl(classification.Id), body);
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent, stringContent);
        
        var storedClassification = dbContext.Classification.FirstOrDefault(x => x.Id == classification.Id);
        storedClassification.Should().NotBeNull();
        storedClassification?.Name.Should().Be(classification.Name);
        
        var storedMetadataRelation = storedClassification?.MetadataTypes.FirstOrDefault(x => x.Name == metadataTypeName);
        storedMetadataRelation.Should().NotBeNull();
    }
    
    [Theory]
    [InlineAutoMoq(ValidClassificationName)]
    public async Task UpsertClassification_UpdateWithBusyName_ShouldReturnConflict(string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var classification = dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertClassificationRequest(
                Name: classificationName, 
                MetadataTypes: Array.Empty<string>())
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertClassificationUrl(Guid.NewGuid()), body);
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict, stringContent);
    }
}