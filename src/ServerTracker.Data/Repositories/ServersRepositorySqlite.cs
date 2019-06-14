using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ServerTracker.Data.Models;

namespace ServerTracker.Data.Repositories
{
    public class ServersRepositorySqlite : IServersRepository
    {
        private string ConnectionString { get; }

        public ServersRepositorySqlite(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task CreateServer(Server server)
        {
            const string sql = @"
                INSERT INTO [servers] (
                    [EnvironmentId],
                    [Name],
                    [DomainName],
                    [IpAddress],
                    [OperatingSystem]
                ) VALUES (
                    @" + nameof(Server.EnvironmentId) + @",
                    @" + nameof(Server.Name) + @",
                    @" + nameof(Server.DomainName) + @",
                    @" + nameof(Server.IpAddress) + @",
                    @" + nameof(Server.OperatingSystem) + @"
                );";

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, server).ConfigureAwait(false);
            }
        }

        public async Task<List<Server>> GetAllServers()
        {
            const string sql = @"
                SELECT *
                  FROM [servers]
                ;";

            List<Server> servers;
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<Server>(sql).ConfigureAwait(false);
                servers = result.ToList();
            }

            return servers;
        }

        public async Task<Server> GetServer(long id)
        {
            const string sql = @"
                SELECT *
                  FROM [servers]
                 WHERE [Id] = @" + nameof(id) + @"
                ;";

            Server server;
            using (var conn = GetConnection())
            {
                server = await conn.QueryFirstOrDefaultAsync<Server>(sql, new {id}).ConfigureAwait(false);
            }

            return server;
        }

        public async Task<List<Server>> GetServersForEnvironment(long envId)
        {
            const string sql = @"
                SELECT *
                  FROM [servers]
                 WHERE [EnvironmentId] = @" + nameof(envId) + @"
                ;";

            List<Server> servers;
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<Server>(sql, new {envId}).ConfigureAwait(false);
                servers = result.ToList();
            }

            return servers;
        }

        public async Task RemoveServer(long id)
        {
            const string sql = @"
                DELETE FROM [servers]
                      WHERE [Id] = @" + nameof(id) + @"
                ;";

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, new {id}).ConfigureAwait(false);
            }
        }

        public async Task UpdateServer(Server server)
        {
            const string sql = @"
                UPDATE [servers]
                   SET [EnvironmentId]   = @" + nameof(Server.EnvironmentId) + @",
                       [Name]            = @" + nameof(Server.Name) + @",
                       [DomainName]      = @" + nameof(Server.DomainName) + @",
                       [IpAddress]       = @" + nameof(Server.IpAddress) + @",
                       [OperatingSystem] = @" + nameof(Server.OperatingSystem) + @"
                 WHERE [Id] = @" + nameof(Server.Id) + @"
                ;";

            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(sql, server).ConfigureAwait(false);
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
