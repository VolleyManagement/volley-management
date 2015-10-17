namespace VolleyManagement.Data.MsSql.Context
{
    using System.Data.Entity;
    using VolleyManagement.Data.MsSql.Context.Migrations;

    /// <summary>
    /// The volley management DB initializer.
    /// </summary>
    internal class VolleyManagementDatabaseInitializer
        : MigrateDatabaseToLatestVersion<VolleyManagementEntities, VolleyContextConfiguration>
    {
    }
}