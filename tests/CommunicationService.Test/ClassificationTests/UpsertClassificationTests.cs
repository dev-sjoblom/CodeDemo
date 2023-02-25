using CommunicationService.Test.ClassificationTests.Fundamental;
using CommunicationService.Test.ClassificationTests.Requests;
using CommunicationService.Test.Fundamental.Helpers;

namespace CommunicationService.Test.ClassificationTests;

public partial class ClassificationTests
{
    private string UpsertClassificationUrl(Guid id) => $"/Classification/{id}";

    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName)]
    public async Task UpsertClassification_NewRegistration_ShouldReturnCreated(string classificationName, string metadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        dbContext.AddMetadataType(metadataTypeName);
        await dbContext.SaveChangesAsync();
        var url = UpsertClassificationUrl(Guid.NewGuid());

        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes:new[] { metadataTypeName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(url, body);
        var stringContent = await response.Content.ReadAsStringAsync();
        
        // assert
        var responseObject = await ValidateClassificationResponse(response, 
            HttpStatusCode.Created);

        responseObject.Should().NotBeNull();
        responseObject.Name.Should().Be(classificationName);
        responseObject.MetadataTypes.Length.Should().Be(1);
        responseObject.MetadataTypes[0].Should().Be(metadataTypeName);
    }
       
    [Theory]
    [PopulateArguments(ValidClassificationName, ValidMetadataTypeName,
        $"A{ValidClassificationName}", $"A{ValidMetadataTypeName}")]
    public async Task UpsertClassification_UpdateInformation_ShouldReturnNoContent(
        string updateClassificationName, string updateMetadataTypeName,
        string registeredClassificationName, string registeredOtherMetadataTypeName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        dbContext.AddMetadataType(updateMetadataTypeName);
        var classification = dbContext.AddClassificationWithMetadata(registeredClassificationName, registeredOtherMetadataTypeName);
        await dbContext.SaveChangesAsync();
        
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertClassificationRequestParameters(
                Name: updateClassificationName, 
                MetadataTypes:new[] { updateMetadataTypeName })
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(UpsertClassificationUrl(classification.Id), body);
        
        // assert
        await ValidateResponse(response, HttpStatusCode.NoContent);
        
        var storedClassification = dbContext.Classification.FirstOrDefault(x => x.Id == classification.Id);
        storedClassification.Should().NotBeNull();
        storedClassification?.Name.Should().Be(classification.Name);
        
        var storedMetadataRelation = storedClassification?.MetadataTypes.FirstOrDefault(x => x.Name == updateMetadataTypeName);
        storedMetadataRelation.Should().NotBeNull();
    }
    
    [Theory]
    [PopulateArguments(ValidClassificationName)]
    public async Task UpsertClassification_UpdateWithBusyName_ShouldReturnConflict(string classificationName)
    {
        // arr
        var dbContext = Fixture.CreateDbContext();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var classification = dbContext.AddClassification(classificationName);
        await dbContext.SaveChangesAsync();
        var url = UpsertClassificationUrl(Guid.NewGuid());
        var client = Fixture.GetMockedClient(dbContext);
        var body = new UpsertClassificationRequestParameters(
                Name: classificationName, 
                MetadataTypes: Array.Empty<string>())
            .AsJsonStringContent();

        // act
        var response = await client.PutAsync(url, body);
        
        // assert
        await ValidateResponseProblem(response, HttpStatusCode.Conflict, 
            "Classification name already taken");
    }
}