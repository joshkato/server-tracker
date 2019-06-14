using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerTracker.Data.Repositories
{
    using Environment = Models.Environment;

    public class EnvironmentsRepositoryInMemory : IEnvironmentsRepository
    {
        private static ConcurrentDictionary<long, Environment> Environments { get; } =
            new ConcurrentDictionary<long, Environment>
            {
                // To make development easier, we'll start with an environment that already exists.
                [1] = new Environment
                {
                    Id = 1,
                    Name = "Development",
                    CreatedAt = DateTime.Now,
                }
            };

        public Task CreateEnvironment(Environment env)
        {
            // Force sequential IDs for environments.
            bool added;
            do
            {
                env.Id = Environments.Count == 0
                    ? 1
                    : Environments.Keys.Max() + 1;

                added = Environments.TryAdd(env.Id, env);

            } while (!added);

            return Task.CompletedTask;
        }

        public Task<List<Environment>> GetAllEnvironments()
        {
            var envs = Environments.Values.OrderBy(e => e.Id).ToList();
            return Task.FromResult(envs);
        }

        public Task RemoveEnvironment(long id)
        {
            Environments.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
