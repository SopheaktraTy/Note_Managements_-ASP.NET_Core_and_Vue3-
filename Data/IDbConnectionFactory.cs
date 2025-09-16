// Data/IDbConnectionFactory.cs
using System.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

// Data/SqlConnectionFactory.cs
using Microsoft.Data.SqlClient;
using System.Data;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _config;
    public SqlConnectionFactory(IConfiguration config) => _config = config;
    public IDbConnection CreateConnection() =>
        new SqlConnection(_config.GetConnectionString("DefaultConnection"));
}
