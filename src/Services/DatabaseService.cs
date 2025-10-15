using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace myapp.Services
{
    public class DatabaseService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DatabaseService(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object? param = null)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync(sql, param);
        }

#pragma warning disable CS8603 // Possible null reference return.
        public async Task<dynamic?> QueryFirstOrDefaultAsync(string sql, object? param = null)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync(sql, param);
        }
#pragma warning restore CS8603 // Possible null reference return.

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, param);
        }

#pragma warning disable CS8603 // Possible null reference return.
        public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<T>(sql, param);
        }
#pragma warning restore CS8603 // Possible null reference return.
    }
}
