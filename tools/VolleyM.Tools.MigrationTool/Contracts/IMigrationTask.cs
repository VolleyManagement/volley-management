using System.Threading.Tasks;

namespace VolleyM.Tools.AzureStorageMigrator.Contracts
{
    /// <summary>
    /// Task to migrate data
    /// </summary>
    public interface IMigrationTask
    {
        Task Initialize();

        Task MigrateUp();
    }
}