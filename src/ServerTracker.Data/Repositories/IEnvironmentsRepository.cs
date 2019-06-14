using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerTracker.Data.Repositories
{
    using Environment = Models.Environment;

    public interface IEnvironmentsRepository
    {
        Task CreateEnvironment(Environment env);

        Task<List<Environment>> GetAllEnvironments();

        Task RemoveEnvironment(long id);
    }
}
