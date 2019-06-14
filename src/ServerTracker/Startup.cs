using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerTracker.Data;
using ServerTracker.Data.Repositories;
using ServerTracker.Hubs;

namespace ServerTracker
{
    public class Startup
    {
        private ILogger Log { get; }

        public IConfiguration Configuration { get; }

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            Log = logger;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSignalR();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            var dataSourceConfig = Configuration.GetValue<string>("DataSource");
            Log.LogInformation("Using data source configuration: {dataSource}", dataSourceConfig);

            switch (dataSourceConfig)
            {
                case "sqlite":
                    UseSqliteDataSource(services);
                    break;
                default:
                    UseInMemoryDataStore(services);
                    break;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IDatabaseBootstrapper dbBootstrapper)
        {
            dbBootstrapper.BootstrapDatabase();

            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseEndpoints(builder =>
            {
                builder.MapHub<ServerTrackerHub>("/ws/server-tracker");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private void UseInMemoryDataStore(IServiceCollection services)
        {
            services.AddSingleton<IDatabaseBootstrapper, DatabaseBootstrapperInMemory>();
            services.AddSingleton<IEnvironmentsRepository, EnvironmentsRepositoryInMemory>();
            services.AddSingleton<IServersRepository, ServersRepositoryInMemory>();
        }

        private void UseSqliteDataSource(IServiceCollection services)
        {

        }
    }
}