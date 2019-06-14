using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ServerTracker
{
    public class Server
    {
        public async Task<int> RunAsync(string[] args)
        {
            IWebHost host;
            try
            {
                host = BuildHost(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to build web host.\n" +
                                  $"{ex.GetType().Name}: {ex.Message}\n" +
                                  $"{ex.StackTrace}");

                return 1;
            }

            await host.RunAsync().ConfigureAwait(false);
            return 0;
        }

        private IWebHost BuildHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
