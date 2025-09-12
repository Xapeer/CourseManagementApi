using System.Data;
using Npgsql;

public class DbContext
{
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}    
