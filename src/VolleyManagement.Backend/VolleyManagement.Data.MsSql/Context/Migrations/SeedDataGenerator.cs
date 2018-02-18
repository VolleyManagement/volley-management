namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using Domain.RolesAggregate;
    using Entities;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Generates and seeds test entity data
    /// </summary>
    internal static class SeedDataGenerator
    {
        private const string ADMINISTRATOR_ROLE_NAME = "Administrator";
        private const string USER_ROLE_NAME = "User";
        private const string TOURNAMENT_ADMINISTRATOR_ROLE_NAME = "TournamentAdministrator";

        /// <summary>
        /// Generates and seeds all required data
        /// </summary>
        /// <param name="context">Context of the entities</param>
        internal static void GenerateRequiredEntities(VolleyManagementEntities context)
        {
            GenerateRoles(context);
            GenerateRolesToOperationsMap(context);
            GenerateContributors(context);
        }

        private static void GenerateContributors(VolleyManagementEntities context)
        {
            var contributorTeams = StoredContributors.GetAllStoredTeamEntities();

            context.ContributorTeams.AddOrUpdate(s => s.Name, contributorTeams.ToArray());
        }

        #region Required

        private static void GenerateRoles(VolleyManagementEntities context)
        {
            var defaultRoles = new List<RoleEntity>
            {
                CreateRole(ADMINISTRATOR_ROLE_NAME),
                CreateRole(TOURNAMENT_ADMINISTRATOR_ROLE_NAME),
                CreateRole(USER_ROLE_NAME)
            };

            context.Roles.AddOrUpdate(r => r.Name, defaultRoles.ToArray());
            context.SaveChanges();
        }

        private static void GenerateRolesToOperationsMap(VolleyManagementEntities context)
        {
            int roleId = context.Roles.Where(r => r.Name == TOURNAMENT_ADMINISTRATOR_ROLE_NAME).First().Id;
            GenerateTournamentAdministratorOperations(roleId, context);

            roleId = context.Roles.Where(r => r.Name == ADMINISTRATOR_ROLE_NAME).First().Id;
            GenerateAdministratorOperations(roleId, context);
        }

        private static void GenerateTournamentAdministratorOperations(int roleId, VolleyManagementEntities context)
        {
            var operationIds = new List<short>
            {
                AuthOperations.Tournaments.Create,
                AuthOperations.Tournaments.Edit,
                AuthOperations.Tournaments.Delete,
                AuthOperations.Tournaments.ManageTeams,
                AuthOperations.Tournaments.ViewArchived,
                AuthOperations.Tournaments.Archive,
                AuthOperations.Teams.Create,
                AuthOperations.Teams.Edit,
                AuthOperations.Teams.Delete,
                AuthOperations.Games.Create,
                AuthOperations.Games.Edit,
                AuthOperations.Games.EditResult,
                AuthOperations.Games.Delete,
                AuthOperations.Games.SwapRounds,
                AuthOperations.Players.Create,
                AuthOperations.Players.Edit,
                AuthOperations.Players.Delete
            };

            var entries = CreateRolesToOperation(roleId, operationIds);

            context.RolesToOperations.AddOrUpdate(r => new { r.RoleId, r.OperationId }, entries.ToArray());
        }

        private static void GenerateAdministratorOperations(int roleId, VolleyManagementEntities context)
        {
            var operationIds = new List<short>
            {
                AuthOperations.Tournaments.Create,
                AuthOperations.Tournaments.Edit,
                AuthOperations.Tournaments.Delete,
                AuthOperations.Tournaments.ManageTeams,
                AuthOperations.Tournaments.ViewArchived,
                AuthOperations.Tournaments.Archive,
                AuthOperations.Teams.Create,
                AuthOperations.Teams.Edit,
                AuthOperations.Teams.Delete,
                AuthOperations.Games.Create,
                AuthOperations.Games.Edit,
                AuthOperations.Games.EditResult,
                AuthOperations.Games.Delete,
                AuthOperations.Games.SwapRounds,
                AuthOperations.Players.Create,
                AuthOperations.Players.Edit,
                AuthOperations.Players.Delete,
                AuthOperations.AdminDashboard.View,
                AuthOperations.AllUsers.ViewList,
                AuthOperations.AllUsers.ViewDetails,
                AuthOperations.AllUsers.ViewActiveList,
                AuthOperations.Feedbacks.Edit,
                AuthOperations.Feedbacks.Delete,
                AuthOperations.Feedbacks.Read,
                AuthOperations.Feedbacks.Reply,
                AuthOperations.Feedbacks.Close,
                AuthOperations.TournamentRequests.ViewList,
                AuthOperations.TournamentRequests.Confirm,
                AuthOperations.TournamentRequests.Decline,
                AuthOperations.Requests.Confirm,
                AuthOperations.Requests.Decline,
                AuthOperations.Requests.ViewList
            };

            var entries = CreateRolesToOperation(roleId, operationIds);

            context.RolesToOperations.AddOrUpdate(r => new { r.RoleId, r.OperationId }, entries.ToArray());
        }

        private static List<RoleToOperationEntity> CreateRolesToOperation(int roleId, List<short> operationIds)
        {
            var entries = new List<RoleToOperationEntity>();
            foreach (var operationId in operationIds)
            {
                entries.Add(new RoleToOperationEntity { RoleId = roleId, OperationId = operationId });
            }

            return entries;
        }

        private static RoleEntity CreateRole(string name)
        {
            return new RoleEntity { Name = name };
        }

        #endregion
    }
}
