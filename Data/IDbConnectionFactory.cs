using System.Data;

namespace NotesApi.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
