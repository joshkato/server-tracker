using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServerTracker.Data.Models;
using ServerTracker.Data.Repositories;

namespace ServerTracker.Data.Services
{
    using Environment = Models.Environment;

    public interface IEnvironmentsService
    {
        Task<ServiceError> AddNewEnvironment(Environment env);
        Task<ServiceError> DeleteEnvironment(long id);
        Task<(List<Environment>, ServiceError)> GetAllEnvironments();
    }

    public class EnvironmentsService : IEnvironmentsService
    {
        private ILogger Log { get; }

        private IEnvironmentsRepository EnvsRepo { get; }

        public EnvironmentsService(ILogger<EnvironmentsService> logger, IEnvironmentsRepository envsRepo)
        {
            Log = logger;
            EnvsRepo = envsRepo;
        }

        public async Task<ServiceError> AddNewEnvironment(Environment env)
        {
            if (env == null)
            {
                Log.LogInformation("Rejecting new environment: provided instance is null.");

                return new ServiceError
                {
                    Message = "Cannot add empty or invalid environment.",
                };
            }

            if (string.IsNullOrWhiteSpace(env.Name))
            {
                Log.LogInformation("Rejecting new environment: missing name.");

                return new ServiceError
                {
                    Message = "New environment is missing, or has an, invalid name.",
                };
            }

            try
            {
                // Clean up the name before we actually add it to the data store.
                env.Name = env.Name.Trim();

                await EnvsRepo.AddNewEnvironment(env).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Failed to add new environment {name}", env.Name);

                return new ServiceError
                {
                    Message = $"Failed to add new environment '{env.Name}'",
                    Exception = ex,
                };
            }

            return null;
        }

        public async Task<ServiceError> DeleteEnvironment(long id)
        {
            try
            {
                await EnvsRepo.DeleteEnvironment(id).ConfigureAwait(false);
                Log.LogDebug("Deleted environment with ID: {id}", id);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Failed to delete environment with ID {id}", id);

                return new ServiceError
                {
                    Message = $"Failed to delete environment with ID {id}",
                    Exception = ex,
                };
            }

            return null;
        }

        public async Task<(List<Environment>, ServiceError)> GetAllEnvironments()
        {
            List<Environment> envs;
            try
            {
                envs = await EnvsRepo.GetAllEnvironments().ConfigureAwait(false);
                Log.LogDebug("{methodName} returned {count} environments.", nameof(GetAllEnvironments), envs.Count);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Failed to retrieve all environments.");

                return (null, new ServiceError
                {
                    Message = "Failed to retrieve all environments",
                    Exception = ex,
                });
            }

            return (envs, null);
        }


    }
}
