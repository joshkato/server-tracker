using System.Threading.Tasks;

namespace ServerTracker
{
    public class Program
    {
        private static Server Server { get; } = new Server();

        public static async Task<int> Main(string[] args)
        {
            return await Server.RunAsync(args).ConfigureAwait(false);
        }
    }
}
