using Npgsql;

namespace CommunicationService.Fundamental;

public static class DatabaseIndexHelper
{
    public static bool IsDatabaseIndexException(this DbUpdateException exception, string indexName)
    {
        if (exception.InnerException is not PostgresException postgresException) 
            return false;
        
        var message = postgresException.Message;
        return message.Contains(indexName);
    }
}