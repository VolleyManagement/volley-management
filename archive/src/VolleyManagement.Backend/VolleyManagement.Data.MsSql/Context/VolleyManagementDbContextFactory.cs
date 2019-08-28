using System.Data.Entity.Infrastructure;

namespace VolleyManagement.Data.MsSql.Context
{
    /// <summary>
    /// Context factory used by Migrations to instantiate DbContext when outside of regular runtime, e.g. from Package Manager console command
    /// </summary>
    public class VolleyManagementDbContextFactory : IDbContextFactory<VolleyManagementEntities>
    {
        public static string ConnectionNameOrString { get; set; } = "VolleyManagementEntities";

        public VolleyManagementEntities Create()
        {
            return new VolleyManagementEntities(ConnectionNameOrString);
        }
    }
}