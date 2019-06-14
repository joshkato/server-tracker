using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerTracker.Data.Repositories
{
    using Environment = ServerTracker.Data.Models.Environment;

    public class EnvironmentsRepositorySqlite : IEnvironmentsRepository
    {
        public Task CreateEnvironment(Environment env)
        {
            throw new NotImplementedException();
        }

        public Task<List<Environment>> GetAllEnvironments()
        {
            throw new NotImplementedException();
        }

        public Task RemoveEnvironment(long id)
        {
            throw new NotImplementedException();
        }
    }
}
