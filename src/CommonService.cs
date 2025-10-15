using Dapper;
using myapp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp
{
    public class CommonService
    {
        private readonly DatabaseService _dbService;

        public CommonService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<IEnumerable<dynamic>> GetListAsync(string tableName, Dictionary<string, string> searchParams)
        {
            var sql = new StringBuilder($"SELECT * FROM {tableName} WHERE 1=1");
            var parameters = new DynamicParameters();

            foreach (var param in searchParams)
            {
                sql.Append($" AND {param.Key} = @{param.Key}");
                parameters.Add(param.Key, param.Value);
            }

            return await _dbService.QueryAsync(sql.ToString(), parameters);
        }

        public async Task<dynamic?> GetByIdAsync(string tableName, string id)
        {
            var sql = $"SELECT * FROM {tableName} WHERE id = @id";
            return await _dbService.QueryFirstOrDefaultAsync(sql, new { id });
        }

        public async Task<long> AddAsync(string tableName, Dictionary<string, object> model)
        {
            var columns = string.Join(", ", model.Keys);
            var values = string.Join(", ", model.Keys.Select(k => "@" + k));
            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values}); SELECT last_insert_rowid();";

            var parameters = new DynamicParameters();
            foreach (var item in model)
            {
                parameters.Add(item.Key, item.Value);
            }

            return await _dbService.ExecuteScalarAsync<long>(sql, parameters);
        }

        public async Task<int> UpdateAsync(string tableName, string id, Dictionary<string, object> model)
        {
            var setClause = string.Join(", ", model.Keys.Select(k => $"{k} = @{k}"));
            var sql = $"UPDATE {tableName} SET {setClause} WHERE id = @id";

            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            foreach (var item in model)
            {
                parameters.Add(item.Key, item.Value);
            }

            return await _dbService.ExecuteAsync(sql, parameters);
        }

        public async Task<int> DeleteAsync(string tableName, string id)
        {
            var sql = $"DELETE FROM {tableName} WHERE id = @id";
            return await _dbService.ExecuteAsync(sql, new { id });
        }
    }
}
