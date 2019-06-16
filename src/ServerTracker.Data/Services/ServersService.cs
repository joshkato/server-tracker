using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServerTracker.Data.Models;
using ServerTracker.Data.Repositories;
using ServerTracker.Data.Validation;

namespace ServerTracker.Data.Services
{
    public interface IServersService
    {
        Task<ServiceError> AddNewServer(Server server);
        Task<ServiceError> DeleteServer(long id);
        Task<(List<Server>, ServiceError)> GetAllServers();
        Task<ServiceError> UpdateServer(Server server);
    }

    public class ServersService : IServersService
    {
        private ILogger Log { get; }

        private IServersRepository ServersRepo { get; }

        private IServerValidator ServerValidator { get; }

        public ServersService(ILogger<ServersService> logger, IServersRepository serversRepo, IServerValidator serverValidator)
        {
            Log = logger;
            ServersRepo = serversRepo;
            ServerValidator = serverValidator;
        }

        public async Task<ServiceError> AddNewServer(Server server)
        {
            var validationResult = ServerValidator.Validate(server);
            if (!validationResult.IsValid)
            {
                return new ServiceError
                {
                    Message =
                        $"Provided server data did not pass validation:\n{string.Join("\n", validationResult.ValidationErrors)}",
                };
            }

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
                    Message = "Failed to add new server.",
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
            var validationResult = ServerValidator.Validate(server);
            if (!validationResult.IsValid)
            {
                return new ServiceError
                {
                    Message =
                        $"Provided server data did not pass validation:\n{string.Join("\n", validationResult.ValidationErrors)}",
                };
            }

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
