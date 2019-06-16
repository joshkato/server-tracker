using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ServerTracker.Data.Models;

namespace ServerTracker.Data.Validation
{
    public interface IServerValidator
    {
        ValidationResult Validate(Server server);
    }

    public class ServerValidator : IServerValidator
    {
        private static Regex RegexIpAddress { get; } = new Regex(
            @"[\d]{1,3}\.[\d]{1,3}\.[\d]{1,3}\.[\d]{1,3}",
            RegexOptions.Compiled);

        private ILogger Log { get; }

        public ServerValidator(ILogger<ServerValidator> logger)
        {
            Log = logger;
        }

        public ValidationResult Validate(Server server)
        {
            var result = new ValidationResult();

            if (server == null)
            {
                Log.LogDebug("Rejecting server instance validation: instance is null.");

                result.IsValid = false;
                result.AddError("Server instance is null.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(server.Name))
            {
                Log.LogDebug("Server failed validation: {property} is null, empty, or whitespace",
                    nameof(Server.Name));

                result.IsValid = false;
                result.AddError($"{nameof(Server.Name)} is null, empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(server.DomainName))
            {
                Log.LogDebug("Server failed validation: {property} is null, empty, or whitespace",
                    nameof(Server.DomainName));

                result.IsValid = false;
                result.AddError($"{nameof(Server.DomainName)} is null, empty, or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(server.IpAddress))
            {
                Log.LogDebug("Server failed validation: {property} is null, empty, or whitespace",
                    nameof(Server.IpAddress));

                result.IsValid = false;
                result.AddError($"{nameof(Server.IpAddress)} is null, empty, or whitespace.");
            }
            else if (!RegexIpAddress.IsMatch(server.IpAddress))
            {
                Log.LogDebug("Server failed validation: {property} value of '{ipAddress}' is not a valid IP address.",
                    nameof(Server.IpAddress), server.IpAddress);

                result.IsValid = false;
                result.AddError($"{nameof(Server.IpAddress)} is not a valid IP address.");
            }

            if (string.IsNullOrWhiteSpace(server.OperatingSystem))
            {
                result.IsValid = false;
                result.AddError($"{nameof(Server.OperatingSystem)} is null, empty, or whitespace.");
            }

            return result;
        }
    }
}
