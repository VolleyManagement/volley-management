namespace VolleyManagement.Data.MsSql.Context
{
    using System.Data.Entity;

    /// <summary>
    /// The volley management DB initializer.
    /// </summary>
    internal class VolleyManagementDatabaseInitializer
        : DropCreateDatabaseIfModelChanges<VolleyManagementEntities>
    {
    }
}