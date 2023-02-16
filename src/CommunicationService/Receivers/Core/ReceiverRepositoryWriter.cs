using CommunicationService.Classifications.Core;
using CommunicationService.Classifications.Data;
using CommunicationService.Fundamental.Helpers;
using CommunicationService.MetadataTypes.Core;
using CommunicationService.MetadataTypes.Data;
using CommunicationService.Receivers.Data;
using static CommunicationService.Receivers.Core.ReceiverErrors;

namespace CommunicationService.Receivers.Core;

public class ReceiverRepositoryWriter : IReceiverRepositoryWriter
{
    private CommunicationDbContext DbContext { get; }
    private IReceiverRepositoryReader RepositoryReader { get; }
    private IClassificationRepositoryReader ClassificationRepositoryReader { get; }
    private IMetadataTypeRepositoryReader MetadataTypeRepositoryReader { get; }

    private const string ixReceiverName = "IX_Receiver_UniqueName";

    public ReceiverRepositoryWriter(CommunicationDbContext dbContext,
        IReceiverRepositoryReader repositoryReader,
        IClassificationRepositoryReader classificationRepositoryReader,
        IMetadataTypeRepositoryReader metadataTypeRepositoryReader)
    {
        DbContext = dbContext;
        RepositoryReader = repositoryReader;
        ClassificationRepositoryReader = classificationRepositoryReader;
        MetadataTypeRepositoryReader = metadataTypeRepositoryReader;
    }

    public async Task<ErrorOr<Created>> CreateReceiver(
        Receiver receiver,
        string[] classifications,
        KeyValuePair<string, string>[] metadatas,
        CancellationToken cancellationToken)
    {
        var existingResult = await RepositoryReader.GetReceiverByUniqueName(receiver.UniqueName, cancellationToken);
        if (!existingResult.IsError)
            return NameAlreadyExists;
        if (existingResult.FirstError != NotFound)
            return existingResult.Errors;

        var createdResult = await UpsertReceiver(receiver, classifications, metadatas, cancellationToken);
        if (createdResult.IsError)
            return createdResult.Errors;

        return Result.Created;
    }

    public async Task<ErrorOr<Deleted>> DeleteReceiver(Guid id, CancellationToken cancellationToken)
    {
        var receiverResult = await RepositoryReader.GetReceiverById(id, cancellationToken);
        if (receiverResult.IsError)
            return receiverResult.Errors;
        DbContext.Remove(receiverResult.Value);

        await DbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }

    public async Task<ErrorOr<UpsertedReceiverResult>> UpsertReceiver(
        Receiver data,
        string[] classifications,
        KeyValuePair<string, string>[] metadatas,
        CancellationToken cancellationToken)
    {
        var receiverResult = await RepositoryReader.GetReceiverById(data.Id, cancellationToken);
        var registerAsNew = false;

        if (receiverResult.IsError)
        {
            if (receiverResult.FirstError.Type != ErrorType.NotFound)
                return receiverResult.Errors;
            registerAsNew = true;
        }

        var receiver = registerAsNew ? data : receiverResult.Value;

        if (registerAsNew)
        {
            DbContext.Receiver.Add(receiver);
        }
        else
        {
            receiver.UniqueName = data.UniqueName;
            receiver.Email = data.Email;

            DbContext.Receiver.Update(receiver);
        }

        receiver.Classifications.RemoveAll(x => true);
        foreach (var c in classifications)
        {
            var classification = await ClassificationRepositoryReader.GetClassificationByName(c, cancellationToken);
            if (classification.IsError)
                return classification.Errors;
            receiver.Classifications.Add(classification.Value);
        }

        receiver.Metadatas.RemoveAll(x => true);
        foreach (var metadataItem in metadatas)
        {
            var metadataType =
                await MetadataTypeRepositoryReader.GetMetadataTypeByName(metadataItem.Key, cancellationToken);
            if (metadataType.IsError)
                return metadataType.Errors;

            if (!metadataType.Value.Classifications.Any(x => receiver.Classifications.Any(p => x.Id == p.Id)))
                return MetadataTypeNotAllowed;

            var metadata = new ReceiverMetadata()
            {
                MetadataType = metadataType.Value,
                Receiver = receiver,
                Data = metadataItem.Value
            };

            receiver.Metadatas.Add(metadata);
        }

        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException updateException) when (updateException.IsDatabaseIndexException(ixReceiverName))
        {
            return NameAlreadyExists;
        }

        return new UpsertedReceiverResult(registerAsNew);
    }
}