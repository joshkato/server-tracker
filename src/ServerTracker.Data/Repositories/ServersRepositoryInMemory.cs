using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServerTracker.Data.Models;

namespace ServerTracker.Data.Repositories
{
    public class ServersRepositoryInMemory : IServersRepository
    {
        public static ConcurrentDictionary<long, Server> Servers { get; } =
            new ConcurrentDictionary<long, Server>
            {
                // To make development easier we'll start with a single server in the environment that
                // should already exist
                [1] = new Server
                {
                    Id = 1,
                    EnvironmentId = 1,
                    Name = "Development Server",
                    DomainName = "dev.test.com",
                    IpAddress = "127.0.0.1",
                    OperatingSystem = "linux-x64",
                    CreatedAt = DateTime.Now,
                }
            };

        public Task AddNewServer(Server server)
        {
            server.CreatedAt = DateTime.Now;

            bool added;
            do
            {
                server.Id = Servers.Count == 0
                    ? 1
                    : Servers.Keys.Max() + 1;

                added = Servers.TryAdd(server.Id, server);

            } while (!added);

            return Task.CompletedTask;
        }

        public Task<List<Server>> GetAllServers()
        {
            var servers = Servers.Values.OrderBy(s => s.Id).ToList();
            return Task.FromResult(servers);
        }

        public Task<Server> GetServer(long id)
        {
            Servers.TryGetValue(id, out var server);
            return Task.FromResult(server);
        }

        public Task<List<Server>> GetServersForEnvironment(long envId)
        {
            var servers = Servers.Values.Where(s => s.EnvironmentId == envId)
                .OrderBy(s => s.Id)
                .ToList();

            return Task.FromResult(servers);
        }

        public Task DeleteServer(long id)
        {
            Servers.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Task UpdateServer(Server server)
        {
            Servers.AddOrUpdate(server.Id, server, (k, v) => server);
            return Task.CompletedTask;
        }
    }
}
