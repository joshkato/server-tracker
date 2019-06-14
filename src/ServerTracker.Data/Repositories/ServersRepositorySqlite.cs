using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
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

        public Task CreateServer(Server server)
        {
            throw new NotImplementedException();
        }

        public Task<List<Server>> GetAllServers()
        {
            throw new NotImplementedException();
        }

        public Task<Server> GetServer(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Server>> GetServersForEnvironment(long envId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveServer(long id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateServer(Server server)
        {
            throw new NotImplementedException();
        }

        private SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(ConnectionString);
            conn.Open();

            return conn;
        }
    }
}
