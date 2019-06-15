using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ServerTracker.Data.Models;
using ServerTracker.Data.Services;
using ServerTracker.Messages;

namespace ServerTracker.Hubs
{
    using Server = Data.Models.Server;

    public class ServerTrackerHub : Hub
    {
        private const string DISPATCH_MESSAGE = "DispatchMessage";

        private ILogger Log { get; }

        private IEnvironmentsService EnvsSvc { get; }

        private IServersService ServersSvc { get; }

        public ServerTrackerHub(ILogger<ServerTrackerHub> logger, IEnvironmentsService envsSvc, IServersService serversSvc)
        {
            Log = logger;

            EnvsSvc = envsSvc;
            ServersSvc = serversSvc;
        }

        public async Task CreateNewEnvironment(string envName)
        {
            var env = new Environment
            {
                Name = envName,
            };

            Log.LogInformation("Adding new environment with name {name}", env.Name);

            var error = await EnvsSvc.AddNewEnvironment(env).ConfigureAwait(false);
            if (error != null)
            {
                await SendErrorToCaller(error).ConfigureAwait(false);
                return;
            }

            await UpdateAvailableEnvironments().ConfigureAwait(false);
        }

        public async Task CreateNewServer(Server server)
        {
            Log.LogInformation("Creating new server: {serverName} ({domainName})", server.Name, server.DomainName);

            var error = await ServersSvc.AddNewServer(server).ConfigureAwait(false);
            if (error != null)
            {
                await SendErrorToCaller(error).ConfigureAwait(false);
                return;
            }

            await UpdateAvailableServers().ConfigureAwait(false);
        }

        public async Task GetAllEnvironments()
        {
            var (environments, error) = await EnvsSvc.GetAllEnvironments().ConfigureAwait(false);
            if (error != null)
            {
                await SendErrorToCaller(error).ConfigureAwait(false);
                return;
            }

            await DispatchToCaller(MessageTypes.ENV_RECV_ALL, environments).ConfigureAwait(false);
        }

        public async Task GetAllServers()
        {
            var (servers, error) = await ServersSvc.GetAllServers().ConfigureAwait(false);
            if (error != null)
            {
                await SendErrorToCaller(error).ConfigureAwait(false);
                return;
            }

            await DispatchToCaller(MessageTypes.SERVERS_RECV_ALL, servers).ConfigureAwait(false);
        }

        public async Task RemoveEnvironment(long id)
        {
            Log.LogInformation("Removing existing environment with ID {id}", id);

            var error = await EnvsSvc.DeleteEnvironment(id).ConfigureAwait(false);
            if (error != null)
            {
                await SendErrorToCaller(error).ConfigureAwait(false);
                return;
            }

            await UpdateAvailableEnvironments().ConfigureAwait(false);
        }

        public async Task RemoveServer(long id)
        {
            Log.LogInformation("Removing existing server with ID {id}", id);

            var error = await ServersSvc.DeleteServer(id).ConfigureAwait(false);
            if (error != null)
            {
                await SendErrorToCaller(error).ConfigureAwait(false);
                return;
            }

            await UpdateAvailableServers().ConfigureAwait(false);
        }

        public async Task UpdateServer(Server server)
        {
            Log.LogInformation("Updating server with ID {id}", server.Id);

            var error = await ServersSvc.UpdateServer(server).ConfigureAwait(false);
            if (error != null)
            {
                await SendErrorToCaller(error).ConfigureAwait(false);
                return;
            }

            await UpdateAvailableServers().ConfigureAwait(false);
        }

        private Task DispatchToCaller<T>(string actionType, T data)
        {
            var message = ClientMessage.ForData(actionType, data);
            return Clients.Caller.SendAsync(DISPATCH_MESSAGE, message);
        }

        private Task DispatchToAll<T>(string actionType, T data)
        {
            var message = ClientMessage.ForData(actionType, data);
            return Clients.All.SendAsync(DISPATCH_MESSAGE, message);
        }

        private async Task SendErrorToCaller(ServiceError error)
        {
            var errResponse = ServerErrorMessage.FromServiceError(error);
            await Clients.Caller.SendAsync(DISPATCH_MESSAGE, errResponse).ConfigureAwait(false);
        }

        private async Task UpdateAvailableEnvironments()
        {
            var (environments, error) = await EnvsSvc.GetAllEnvironments().ConfigureAwait(false);
            if (error != null)
            {
                // Since this is a global update for anyone connected to the server, we don't
                // need to notify everyone when it fails. For now, we'll let the underlying
                // service log the error then not do anything for the clients.
                return;
            }

            Log.LogTrace("Updating all clients with {count} environments.", environments.Count);
            await DispatchToAll(MessageTypes.ENV_RECV_ALL, environments).ConfigureAwait(false);
        }

        private async Task UpdateAvailableServers()
        {
            var (servers, error) = await ServersSvc.GetAllServers().ConfigureAwait(false);
            if (error != null)
            {
                // Since this is a global update for anyone connected to the server, we don't
                // need to notify everyone when it fails. For now, we'll let the underlying
                // service log the error then not do anything for the clients.
                return;
            }

            Log.LogTrace("Updating all clients with {count} servers.", servers.Count);
            await DispatchToAll(MessageTypes.SERVERS_RECV_ALL, servers).ConfigureAwait(false);
        }

        private static class MessageTypes
        {
            public const string ENV_RECV_ALL     = "ENV_RECV_ALL";
            public const string SERVERS_RECV_ALL = "SERVERS_RECV_ALL";
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
