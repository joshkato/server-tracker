using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ServerTracker.Data
{
    public class DatabaseBootstrapperSqlite : IDatabaseBootstrapper
    {
        private ILogger Log { get; }

        private SQLiteConnectionStringBuilder ConnectionString { get; }

        public DatabaseBootstrapperSqlite(ILogger<DatabaseBootstrapperSqlite> logger, string connectionString)
        {
            Log = logger;
            ConnectionString = new SQLiteConnectionStringBuilder(connectionString);
        }

        public void BootstrapDatabase()
        {
            const string createEnvironmentsTable = @"
                create table if not exists environments
                (
	                Id integer not null
		                constraint environments_pk
			                primary key autoincrement,
	                Name VARCHAR(100) default '' not null,
	                CreatedAt TIMESTAMP default CURRENT_TIMESTAMP not null
                );

                create unique index if not exists environments_Name_uindex
	                on environments (Name);
                ";

            const string createServersTable = @"
                create table if not exists servers
                (
	                Id integer not null
		                constraint servers_pk
			                primary key autoincrement,
	                EnvironmentId integer not null,
	                Name varchar(100) default '' not null,
	                DomainName varchar(250) default '' not null,
	                IpAddress varchar(20) default '127.0.0.1' not null,
	                OperatingSystem varchar(100) default 'Unknown' not null,
	                CreatedAt timestamp default current_timestamp not null
                );
                ";

            Log.LogInformation("Bootstrapping database at {dataSource}", ConnectionString.DataSource);
            using (var conn = new SQLiteConnection(ConnectionString.ToString()))
            {
                conn.Open();
                conn.Execute(createEnvironmentsTable);
                conn.Execute(createServersTable);

                // We'll make sure we have some kind of initial environment to work with:
                var devEnv = conn.QueryFirstOrDefault<Models.Environment>("SELECT * FROM [environments] WHERE [Name] = @Name;", new {Name = "Development"});
                if (devEnv == null)
                {
                    conn.Execute("INSERT INTO [environments] ([Name]) VALUES ('Development');");
                }

                conn.Close();
            }
        }
    }
}
