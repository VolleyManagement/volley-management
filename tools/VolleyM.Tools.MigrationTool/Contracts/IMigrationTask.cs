using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace VolleyM.Tools.MigrationTool.Contracts
{
    /// <summary>
    /// Task to migrate data
    /// </summary>
    public interface IMigrationTask
    {
        Task Initialize(IConfiguration config);

        Task MigrateUp();
    }
}