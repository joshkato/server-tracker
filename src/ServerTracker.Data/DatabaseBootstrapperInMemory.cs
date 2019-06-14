using Microsoft.Extensions.Logging;

namespace ServerTracker.Data
{
    public class DatabaseBootstrapperInMemory : IDatabaseBootstrapper
    {
        private ILogger Log { get; }

        public DatabaseBootstrapperInMemory(ILogger<DatabaseBootstrapperInMemory> logger)
        {
            Log = logger;
        }

        public void BootstrapDatabase()
        {
            Log.LogInformation("Successfully bootstrapped in-memory data stores.");
            // noop since we don't need to do anything to initialize in-memory data stores.
        }
    }
}
