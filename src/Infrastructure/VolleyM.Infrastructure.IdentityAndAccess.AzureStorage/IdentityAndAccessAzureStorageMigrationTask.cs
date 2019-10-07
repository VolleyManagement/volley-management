using System.Composition;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using VolleyM.Tools.AzureStorageMigrator.Contracts;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    [Export(typeof(IMigrationTask))]
    public class IdentityAndAccessAzureStorageMigrationTask : IMigrationTask
    {
        public Task Initialize(IConfiguration config)
        {
            Log.Information("IaAContext initialization complete.");
            return Task.CompletedTask;
        }

        public Task MigrateUp()
        {
            Log.Information("IaAContext migration complete.");
            return Task.CompletedTask;
        }
    }
}