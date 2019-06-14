using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerTracker.Data.Models;

namespace ServerTracker.Data.Repositories
{
    public class ServersRepositorySqlite : IServersRepository
    {
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
    }
}
