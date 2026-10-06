

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace WCNApi.core.Data
{
    public class AppDbContext 
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task<List<T>> ExecuteStoreProcedureAsync<T>(string storedProcedure, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            var result = new List<T>();
            var parser = reader.GetRowParser<T>();

            while (await reader.ReadAsync())
            {
                result.Add(parser(reader));
            }

            return result;
        }

        public async Task<T?> ExecuteStoreProcedureSingleAsync<T>(string storedProcedure, List<SqlParameter>? parameters = null)
        {
            var list = await ExecuteStoreProcedureAsync<T>(storedProcedure, parameters);
            return list.Count > 0 ? list[0] : default;
        }

        public async Task<DataTable> ExecuteStoreProcedureDataTableAsync(string storedProcedure, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            var table = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            return table;
        }


        public async Task<List<T>> ExecuteQueryAsync<T>(string query, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand(query, connection)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            var result = new List<T>();
            var parser = reader.GetRowParser<T>();

            while (await reader.ReadAsync())
            {
                result.Add(parser(reader));
            }

            return result;
        }

        public async Task<T?> ExecuteQuerySingleAsync<T>(string query, List<SqlParameter>? parameters = null)
        {
            var list = await ExecuteQueryAsync<T>(query, parameters);
            return list.Count > 0 ? list[0] : default;
        }

        public async Task<DataTable> ExecuteQueryDataTableAsync(string query, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand(query, connection)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            var table = new DataTable();
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            return table;
        }

        #region ExcuteNonQuery

        public async Task<int> ExecuteNonQueryAsync<T>(string query, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand(query, connection)
            {
                CommandType = CommandType.Text
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            await connection.OpenAsync();
             int rowsAffected = await command.ExecuteNonQueryAsync();
           
            return rowsAffected;
        }

        #endregion

    }
}
