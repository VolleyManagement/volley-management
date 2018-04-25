using Respawn;
using VolleyManagement.Data.MsSql.Context;
using VolleyManagement.Data.MsSql.Entities;

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
            new VolleyManagementEntities(IntegrationTestConfigurationProvider.GetVolleyManagementEntitiesConnectionString());

        /// <summary>
        /// Cleans DB tables to initial state
        /// </summary> 
        public static void Respawn()
        {
            Checkpoint.Reset(IntegrationTestConfigurationProvider.GetVolleyManagementEntitiesConnectionString()).Wait();
        }

        /// <summary>
        /// Create a new player.
        /// </summary>
        /// <param name="playerEntity">A player to create. Instance of <see cref="PlayerEntity"/></param>
        public static void CreatePlayer(PlayerEntity playerEntity)
        {
            using (Context)
            {
                Context.Players.Add(playerEntity);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="teamEntity">A team to create. Instance of <see cref="TeamEntity"/></param>
        public static void CreateTeam(TeamEntity teamEntity)
        {
            using (Context)
            {
                Context.Teams.Add(teamEntity);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Assign player to team
        /// </summary>
        /// <param name="playerId">A player id.</param>
        /// <param name="teamId">A team id.</param>
        public static void AssignPlayerToTeam(int playerId, int teamId)
        {
            using (Context)
            {
                var playerEntity = Context.Players.Find(playerId);
                if (playerEntity == null)
                {
                    return;
                }

                playerEntity.TeamId = teamId;
                Context.SaveChanges();
            }
        }
    }
}