using System.Collections.Generic;
using System.Threading.Tasks;
using ServerTracker.Data.Models;

namespace ServerTracker.Data.Repositories
{
    public interface IServersRepository
    {
        Task AddNewServer(Server server);

        Task DeleteServer(long id);

        Task<List<Server>> GetAllServers();

        Task<Server> GetServer(long id);

        Task<List<Server>> GetServersForEnvironment(long envId);

        Task UpdateServer(Server server);
    }
}
