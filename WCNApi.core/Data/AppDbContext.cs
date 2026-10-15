

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Data;
using WCNApi.core.Domain.Models;

namespace WCNApi.core.Data
{
    public class AppDbContext : DbContext, IDbContext
    {
        //private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            //_configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("DefaultConnection is not configured.");
        }

        // EF CORE DATABASE ACCESS
        public DatabaseFacade Db => Database;
        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(sql, parameters);
        }
        public void DisposeContext()
        {
            Dispose();
        }

        // DAPPER / ADO.NET CONNECTION
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

        public async Task<int> ExecuteStoreProcedureNonQueryAsync(string storedProcedure, List<SqlParameter>? parameters = null)
        {
            using var connection = GetConnection();

            using var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
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

        // EF CORE TABLE MAPPING
        public DbSet<LoginUserDetailsModel> LoginUsers { get; set; } = null!;
        public DbSet<LoginHistoryModel> LoginHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoginUserDetailsModel>(entity =>
            {
                entity.ToTable("Tbl_Login_Userdetails");
                entity.HasKey(x => x.Uid);
            });

            modelBuilder.Entity<LoginHistoryModel>(entity =>
            {
                entity.ToTable("Tbl_Login_History");
                entity.HasKey(x => x.Lid);
                entity.HasOne(x => x.User)
                    .WithMany(x => x.LoginHistories)
                    .HasForeignKey(x => x.Userid)
                    .HasPrincipalKey(x => x.Uid);
            });
        }
    }
}
