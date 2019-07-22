﻿namespace VolleyManagement.Data.MsSql.Context
{
    /// <summary>
    /// Provides various DB names
    /// </summary>
    public static class VolleyDatabaseMetadata
    {
        public const string DATE_COLUMN_TYPE = "date";

        public const string DATETIME2_COLUMN_TYPE = "datetime2";

        public const string TINYINT_COLUMN_TYPE = "tinyint";

        public const string SMALL_INT_COLUMN_TYPE = "smallint";

        public const string TOURNAMENTS_TABLE_NAME = "Tournaments";

        public const string TOURNAMENT_SCHEME_COLUMN_NAME = "SchemeCode";

        public const string TOURNAMENT_SEASON_COLUMN_NAME = "SeasonOffset";

        public const string FEEDBACKS_TABLE_NAME = "Feedbacks";

        public const string FEEDBACK_STATUS_COLUMN_NAME = "StatusCode";

        public const string PLAYERS_TABLE_NAME = "Players";

        public const string TEAMS_TABLE_NAME = "Teams";

        public const string GROUPS_TO_TEAMS_TABLE_NAME = "GroupTeam";

        public const string GROUP_TO_TEAM_FK = "TeamId";

        public const string TEAM_TO_GROUP_FK = "GroupId";

        public const string CONTRIBUTORS_TABLE_NAME = "Contributors";

        public const string CONTRIBUTOR_TEAMS_TABLE_NAME = "ContributorTeams";

        public const string CONTRIBUTOR_TO_TEAM_FK = "ContributorTeamId";

        public const string USERS_TABLE_NAME = "Users";

        public const string LOGIN_PROVIDERS_TABLE_NAME = "LoginProviders";

        public const string ROLES_TABLE_NAME = "Roles";

        public const string ROLES_TO_OPERATIONS_TABLE_NAME = "RolesToOperationsMap";

        public const string USERS_TO_ROLES_TABLE_NAME = "UserToRoleMap";

        public const string USER_TO_ROLE_FK = "RoleId";

        public const string ROLE_TO_USER_FK = "UserId";

        public const string GROUPS_TABLE_NAME = "Groups";

        public const string DIVISION_TABLE_NAME = "Divisions";

        public const string GAME_RESULTS_TABLE_NAME = "GameResults";

        public const string TOURNAMENT_REQUEST_TABLE_NAME = "TournamentRequests";

        public const string REQUESTS_TABLE_NAME = "Requests";
    }
}
