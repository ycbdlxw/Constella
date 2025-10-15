
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace myapp.Services
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var dbType = _configuration.GetValue<string>("Database:Type");
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database type or connection string is not configured.");
            }

            switch (dbType.ToLower())
            {
                case "sqlite":
                    return new SqliteConnection(connectionString);
                // case "sqlserver":
                //     return new SqlConnection(connectionString); // 需要添加 Microsoft.Data.SqlClient 包
                // case "postgresql":
                //     return new NpgsqlConnection(connectionString); // 需要添加 Npgsql 包
                default:
                    throw new NotSupportedException($"Database type '{dbType}' is not supported.");
            }
        }
    }
}
