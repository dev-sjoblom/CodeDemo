using Npgsql;

namespace CommunicationService.Fundamental.DataAccess;

public static class DatabaseIndexHelper
{
    public static bool IsDatabaseIndexException(this DbUpdateException exception, string indexName)
    {
        ArgumentNullException.ThrowIfNull(exception);
        ArgumentException.ThrowIfNullOrEmpty(indexName);

        if (exception.InnerException is not PostgresException postgresException)
            return false;

        var message = postgresException.Message;
        return message.Contains(indexName);
    }
}