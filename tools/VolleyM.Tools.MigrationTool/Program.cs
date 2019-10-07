using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VolleyM.Tools.MigrationTool.Contracts;

namespace VolleyM.Tools.MigrationTool
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
            IConfiguration config = LoadPluginConfiguration(migrationTask);
            Log.Debug("Migration started for {MigrationName}.", migrationTask.GetType().Name);
            await migrationTask.Initialize(config);
            Log.Debug("Initialized {MigrationName} migration.", migrationTask.GetType().Name);
            await migrationTask.MigrateUp();
            Log.Debug("Migration {MigrationName} complete.", migrationTask.GetType().Name);
        }

        private static IConfiguration LoadPluginConfiguration(IMigrationTask migrationTask)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetAssemblyFolder(migrationTask))
                .AddJsonFile("pluginsettings.json", true)
                .AddJsonFile($"pluginsettings.{GetEnvironmentName()}.json", true)
                .AddEnvironmentVariables("VOLLEYM_MIGRATION_");

            return builder.Build();
        }

        private static string GetAssemblyFolder(IMigrationTask migrationTask)
        {
            var fileInfo = new FileInfo(migrationTask.GetType().Assembly.Location);
            return fileInfo.DirectoryName;
        }

        private static string GetMigrationTasksDirectory()
        {
            return Path.Combine(AppContext.BaseDirectory, "tasks"); ;
        }

        private static void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json", true)
                .AddEnvironmentVariables("VOLLEYM_MIGRATION_");

            _configuration = builder.Build();
        }

        private static string GetEnvironmentName()
        {
            return _configuration["EnvironmentName"] ?? "Development";
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
