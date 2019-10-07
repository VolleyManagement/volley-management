using System.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;
using VolleyM.Tools.AzureStorageMigrator.Contracts;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    [Export(typeof(IMigrationTask))]
    public class IdentityAndAccessAzureStorageMigrationTask : IMigrationTask
    {
        private IdentityContextTableStorageOptions _options;
        private TableConfiguration.TableConfiguration _tableConfig;

        public Task Initialize(IConfiguration config)
        {
            _options = config.GetSection("IdentityContextTableStorageOptions")
                .Get<IdentityContextTableStorageOptions>();

            _tableConfig = new TableConfiguration.TableConfiguration(_options);

            Log.Information("IaAContext initialization complete.");
            return Task.CompletedTask;
        }

        public async Task MigrateUp()
        {
            await _tableConfig.ConfigureTables();
            Log.Information("IaAContext migration complete.");
        }
    }
}