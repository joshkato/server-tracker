using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace ServerTracker.Data.Repositories
{
    using Environment = Models.Environment;

    public class EnvironmentsRepositorySqlite : IEnvironmentsRepository
    {
        private string ConnectionString { get; }

        public EnvironmentsRepositorySqlite(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task AddNewEnvironment(Environment env)
        {
            const string sql = @"
                INSERT INTO [environments] (
                    [Name]
                ) VALUES (
                    @" + nameof(Environment.Name) + @"
                );";

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, env).ConfigureAwait(false);
            }
        }

        public async Task<List<Environment>> GetAllEnvironments()
        {
            const string sql = @"
                SELECT *
                  FROM [environments]
                ;";

            List<Environment> environments;
            using (var conn = GetConnection())
            {
                var results = await conn.QueryAsync<Environment>(sql).ConfigureAwait(false);
                environments = results.ToList();
            }

            return environments;
        }

        public async Task DeleteEnvironment(long id)
        {
            const string sql = @"
                DELETE FROM [environments]
                      WHERE [Id] = @" + nameof(id) + @"
                ;";

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, new {id}).ConfigureAwait(false);
            }
        }

        private SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(ConnectionString);
            conn.Open();

            return conn;
        }
    }
}
