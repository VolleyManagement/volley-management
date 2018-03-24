using Respawn;
using VolleyManagement.Data.MsSql.Context;

namespace VolleyManagement.Specs.Infrastructure
{
    /// <summary>
    /// Provides direct access to DB in order to assert state avoiding using business logic
    /// </summary>
    public static class TestDbAdapter
    {
        private static readonly Checkpoint Checkpoint = new Checkpoint {
            TablesToIgnore = new[]
            {
                "__MigrationHistory",
                VolleyDatabaseMetadata.CONTRIBUTORS_TABLE_NAME,
                VolleyDatabaseMetadata.CONTRIBUTOR_TEAMS_TABLE_NAME,
                VolleyDatabaseMetadata.ROLES_TABLE_NAME,
                VolleyDatabaseMetadata.ROLES_TO_OPERATIONS_TABLE_NAME,
                VolleyDatabaseMetadata.USERS_TABLE_NAME,
                VolleyDatabaseMetadata.USERS_TO_ROLES_TABLE_NAME,
            }
        };

        public static VolleyManagementEntities Context =>
            new VolleyManagementEntities(IntegrationTestConfigurationProvider.IT_CONNECTION_STRING);

        /// <summary>
        /// Cleans DB tables to initial state
        /// </summary> 
        public static void Respawn()
        {
            Checkpoint.Reset(IntegrationTestConfigurationProvider.IT_CONNECTION_STRING).Wait();
        }
    }
}