using Microsoft.Extensions.Configuration;
using Serilog;
using System.Composition;
using System.Threading.Tasks;
using VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration;
using VolleyM.Tools.MigrationTool.Contracts;

namespace VolleyM.Infrastructure.Players.AzureStorage
{
    [Export(typeof(IMigrationTask))]
    public class PlayersAzureStorageMigrationTask : IMigrationTask
    {
        private PlayersContextTableStorageOptions _options;
        private TableConfiguration.TableConfiguration _tableConfig;

        public Task Initialize(IConfiguration config)
        {
            _options = config.GetSection("PlayersContextTableStorageOptions")
                .Get<PlayersContextTableStorageOptions>();

            _tableConfig = new TableConfiguration.TableConfiguration(_options);

            Log.Information("PlayersContext initialization complete.");
            return Task.CompletedTask;
        }

        public async Task MigrateUp()
        {
            await _tableConfig.ConfigureTables();
            Log.Information("PlayersContext migration complete.");
        }
    }
}