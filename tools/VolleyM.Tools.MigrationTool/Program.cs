using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using VolleyM.Tools.AzureStorageMigrator.Contracts;

namespace VolleyM.Tools.AzureStorageMigrator
{
    public class Program
    {
        private static IConfiguration _configuration;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("AzureStorageMigrator started.");
            InitConfiguration();

            ConfigureLogger(_configuration);
            Log.Debug("Logger configured.");

            var discoverer = new MigrationTaskDiscoverer();

            discoverer.Discover(GetMigrationTasksDirectory());
            
            await Task.WhenAll(discoverer.MigrationTasks.Select(RunMigrationTask));
            Log.Information("AzureStorageMigrator completed.");
        }

        private static async Task RunMigrationTask(IMigrationTask migrationTask)
        {
            Log.Debug("Migration started for {MigrationName}.", migrationTask.GetType().Name);
            await migrationTask.Initialize();
            Log.Debug("Initialized {MigrationName} migration.", migrationTask.GetType().Name);
            await migrationTask.MigrateUp();
            Log.Debug("Migration {MigrationName} complete.", migrationTask.GetType().Name);
        }

        private static string GetMigrationTasksDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "migrationTasks");
        }

        private static void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json", true)
                .AddEnvironmentVariables("VOLLEYM_");

            _configuration = builder.Build();
        }

        private static void ConfigureLogger(IConfiguration config)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(config)
                .WriteTo.Console()
                .WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    "azure-storage-migrator.log",
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10 * 1024 * 1024/*10 MB*/,
                    retainedFileCountLimit: 10)
                .CreateLogger();

            Log.Logger = logger.ForContext("SourceContext", "AzureStorageMigrator");
        }
    }
}
