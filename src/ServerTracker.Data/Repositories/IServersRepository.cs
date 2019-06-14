using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServerTracker.Data.Models;

namespace ServerTracker.Data.Repositories
{
    public interface IServersRepository
    {
        Task CreateServer(Server server);

        Task<List<Server>> GetAllServers();

        Task<Server> GetServer(long id);

        Task<List<Server>> GetServersForEnvironment(long envId);

        Task RemoveServer(long id);

        Task UpdateServer(Server server);
    }
}
