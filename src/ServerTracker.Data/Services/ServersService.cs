using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServerTracker.Data.Models;
using ServerTracker.Data.Repositories;

namespace ServerTracker.Data.Services
{
    public interface IServersService
    {
        Task<ServiceError> AddNewServer(Server server);
        Task<ServiceError> DeleteServer(long id);
        Task<(List<Server>, ServiceError)> GetAllServers();
        Task<Server> GetServer(long id);
        Task<ServiceError> UpdateServer(Server server);
    }

    public class ServersService : IServersService
    {
        private ILogger Log { get; }

        private IServersRepository ServersRepo { get; }

        public ServersService(ILogger<ServersService> logger, IServersRepository serversRepo)
        {
            Log = logger;
            ServersRepo = serversRepo;
        }

        public async Task<ServiceError> AddNewServer(Server server)
        {
            // TODO - Add validation
            try
            {
                await ServersRepo.AddNewServer(server).ConfigureAwait(false);
                Log.LogDebug("Successfully added new server {name} ({domain})", server.Name, server.DomainName);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Failed to add new server: {name} ({domain})", server.Name, server.DomainName);
                return new ServiceError
                {
                    Message = "Failed to add server.",
                    Exception = ex,
                };
            }

            return null;
        }

        public async Task<ServiceError> DeleteServer(long id)
        {
            try
            {
                await ServersRepo.DeleteServer(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Failed to delete server with ID {id}", id);
                return new ServiceError
                {
                    Message = "Failed to delete server",
                    Exception = ex,
                };
            }

            return null;
        }

        public async Task<Server> GetServer(long id)
        {
            return await Task.FromResult((Server)null).ConfigureAwait(false);
        }

        public async Task<(List<Server>, ServiceError)> GetAllServers()
        {
            List<Server> servers;
            try
            {
                servers = await ServersRepo.GetAllServers().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Failed to retrieve all servers.");
                return (null, new ServiceError
                {
                    Message = "Failed to retrieve all servers",
                    Exception = ex,
                });
            }

            return (servers, null);
        }

        public async Task<ServiceError> UpdateServer(Server server)
        {
            // TODO - Add validation
            try
            {
                await ServersRepo.UpdateServer(server).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Failed to update server ID {serverId}", server.Id);
                return new ServiceError
                {
                    Message = "Failed to update server.",
                    Exception = ex,
                };
            }

            return null;
        }
    }
}
