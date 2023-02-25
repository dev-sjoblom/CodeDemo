using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.Fundamental.Helpers;
using CommunicationService.Test.MetadataTypeTests.Model;

namespace CommunicationService.Test.MetadataTypeTests;

public partial class MetadataTypeTests
{
    private string UpsertMetadataTypeUrl(Guid id) => $"/MetadataType/{id}";

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task UpsertMetadataType_NewRegistration_ShouldReturnCreated(string metadataTypeName, string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();

        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertMetadataTypeRequestParameters(
                Name: metadataTypeName, 
                Classifications:new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertMetadataTypeUrl(Guid.NewGuid()), body);
        
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
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var changeToClassification = dbContext.AddClassification(classificationName);
        var metadataType = dbContext.AddMetadataTypeWithClassification("someMetadataType", "someClassification");
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertMetadataTypeRequestParameters(
                Name: metadataTypeName, 
                Classifications:new[] { classificationName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertMetadataTypeUrl(metadataType.Id), body);

        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);
        
        var storedMetadataType = dbContext.MetadataType.FirstOrDefault(x => x.Id == metadataType.Id);
        storedMetadataType.Should().NotBeNull();
        storedMetadataType?.Name.Should().Be(metadataTypeName);
        
        var storedClassificationRelation = storedMetadataType?.Classifications.FirstOrDefault(x => x.Id == changeToClassification.Id);
        storedClassificationRelation.Should().NotBeNull();
    }
    
    [Theory]
    [PopulateArguments(ValidMetadataTypeName)]
    public async Task UpsertMetadataType_UpdateWithBusyName_ShouldReturnConflict(string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataType(metadataTypeName);
        await dbContext.SaveChangesAsync();
        var url = UpsertMetadataTypeUrl(Guid.NewGuid());
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertMetadataTypeRequestParameters(
                Name: metadataTypeName, 
                Classifications: Array.Empty<string>())
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(url, body);
        
        // assert
        await ValidateResponseProblem(response, 
            HttpStatusCode.Conflict, 
            "MetadataType name already taken.");
    }
}