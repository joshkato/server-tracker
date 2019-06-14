using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ServerTracker.Data.Repositories;

namespace ServerTracker.Hubs
{
    using Environment = Data.Models.Environment;
    using Server = Data.Models.Server;

    public class ServerTrackerHub : Hub
    {
        private const string DISPATCH_MESSAGE = "DispatchMessage";

        private ILogger Log { get; }

        private IEnvironmentsRepository EnvsRepo { get; }

        private IServersRepository ServersRepo { get; }

        public ServerTrackerHub(ILogger<ServerTrackerHub> logger, IEnvironmentsRepository envsRepo, IServersRepository serversRepo)
        {
            Log = logger;

            EnvsRepo = envsRepo;
            ServersRepo = serversRepo;
        }

        public async Task CreateNewEnvironment(string envName)
        {
            Log.LogInformation("Creating new environment: {envName}", envName);

            await EnvsRepo.CreateEnvironment(new Environment {Name = envName, CreatedAt = DateTime.Now,})
                .ConfigureAwait(false);

            await UpdateAvailableEnvironments().ConfigureAwait(false);
        }

        public async Task CreateNewServer(Server server)
        {
            Log.LogInformation("Creating new server: {serverName} ({domainName})", server.Name, server.DomainName);

            await ServersRepo.CreateServer(server).ConfigureAwait(false);

            await UpdateAvailableServers();
        }

        public async Task GetAllEnvironments()
        {
            var environments = await EnvsRepo.GetAllEnvironments()
                .ConfigureAwait(false);

            var response = ClientMessage.ForData(MessageTypes.ENV_RECV_ALL, environments);
            await Clients.Caller.SendAsync(DISPATCH_MESSAGE, response).ConfigureAwait(false);
        }

        public async Task GetAllServers()
        {
            var servers = await ServersRepo.GetAllServers().ConfigureAwait(false);

            var response = ClientMessage.ForData(MessageTypes.SERVERS_RECV_ALL, servers);
            await Clients.Caller.SendAsync(DISPATCH_MESSAGE, response).ConfigureAwait(false);
        }

        public async Task GetServersForEnvironment(long envId)
        {
            var servers = await ServersRepo.GetServersForEnvironment(envId).ConfigureAwait(false);

            var response = ClientMessage.ForData(MessageTypes.SERVERS_RECV_FOR_ENV, servers);
            await Clients.Caller.SendAsync(DISPATCH_MESSAGE, response).ConfigureAwait(false);
        }

        public async Task RemoveEnvironment(long id)
        {
            Log.LogInformation("Removing existing environment with ID {id}", id);

            await EnvsRepo.RemoveEnvironment(id).ConfigureAwait(false);

            await UpdateAvailableEnvironments().ConfigureAwait(false);
        }

        public async Task RemoveServer(long id)
        {
            Log.LogInformation("Removing existing server with ID {id}", id);

            await ServersRepo.RemoveServer(id).ConfigureAwait(false);

            await UpdateAvailableServers().ConfigureAwait(false);
        }

        public async Task UpdateServer(Server server)
        {
            Log.LogInformation("Updating server with ID {id}", server.Id);

            await ServersRepo.UpdateServer(server).ConfigureAwait(false);

            await UpdateAvailableServers().ConfigureAwait(false);
        }

        private async Task UpdateAvailableEnvironments()
        {
            var environments = await EnvsRepo.GetAllEnvironments()
                .ConfigureAwait(false);

            var response = ClientMessage.ForData(MessageTypes.ENV_RECV_ALL, environments);
            await Clients.All.SendAsync(DISPATCH_MESSAGE, response).ConfigureAwait(false);
        }

        private async Task UpdateAvailableServers()
        {
            var servers = await ServersRepo.GetAllServers().ConfigureAwait(false);

            var response = ClientMessage.ForData(MessageTypes.SERVERS_RECV_ALL, servers);
            await Clients.All.SendAsync(DISPATCH_MESSAGE, response).ConfigureAwait(false);
        }

        private static class MessageTypes
        {
            public const string ENV_RECV_ALL         = "ENV_RECV_ALL";
            public const string SERVERS_RECV_ALL     = "SERVERS_RECV_ALL";
            public const string SERVERS_RECV_FOR_ENV = "SERVERS_RECV_FOR_ENV";
        }
    }

    public class ClientMessage
    {
        public static ClientMessage<T> ForData<T>(string messageType, T data)
        {
            return new ClientMessage<T>
            {
                Type = messageType,
                Data = data,
            };
        }
    }

    public class ClientMessage<T> : ClientMessage
    {
        public string Type { get; set; }

        public T Data { get; set; }
    }
}
