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
            using (var ctx = Context)
            {
                ctx.Players.Add(playerEntity);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="teamEntity">A team to create. Instance of <see cref="TeamEntity"/></param>
        public static void CreateTeam(TeamEntity teamEntity)
        {
            using (var ctx = Context)
            {
                ctx.Teams.Add(teamEntity);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Assign player to team
        /// </summary>
        /// <param name="playerId">A player id.</param>
        /// <param name="teamId">A team id.</param>
        public static void AssignPlayerToTeam(int playerId, int teamId)
        {
            using (var ctx = Context)
            {
                var playerEntity = ctx.Players.Find(playerId);
                if (playerEntity == null)
                {
                    return;
                }

                playerEntity.TeamId = teamId;
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Create Team with captain
        /// </summary>
        /// <param name="_team"></param>
        /// <param name="captainFirstame"></param>
        /// <param name="captainLastName"></param>
        public static void CreateTeamWithCaptain(TeamEntity _team, string captainFirstame, string captainLastName)
        {
            var player = new PlayerEntity {
                FirstName = captainFirstame,
                LastName = captainLastName
            };
            using (var ctx = Context)
            {
                ctx.Players.Add(player);
                _team.Captain = player;
                ctx.Teams.Add(_team);
                ctx.SaveChanges();
            }
            AssignPlayerToTeam(player.Id, _team.Id);
        }
    }
}