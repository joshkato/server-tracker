using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerTracker.Data.Repositories
{
    using Environment = Models.Environment;

    public interface IEnvironmentsRepository
    {
        Task AddNewEnvironment(Environment env);

        Task DeleteEnvironment(long id);

        Task<List<Environment>> GetAllEnvironments();
    }
}
