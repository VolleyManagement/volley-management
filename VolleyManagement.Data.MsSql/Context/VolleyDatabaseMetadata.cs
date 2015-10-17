namespace VolleyManagement.Data.MsSql.Context
{
    /// <summary>
    /// Provides various DB names
    /// </summary>
    internal static class VolleyDatabaseMetadata
    {
        public const string DATE_COLUMN_TYPE = "date";

        public const string SMALL_INT_COLUMN_TYPE = "smallint";

        public const string TOURNAMENTS_TABLE_NAME = "Tournaments";

        public const string TOURNAMENT_SCHEME_COLUMN_NAME = "SchemeCode";

        public const string TOURNAMENT_SEASON_COLUMN_NAME = "SeasonOffset";

        public const string PLAYERS_TABLE_NAME = "Players";

        public const string TEAMS_TABLE_NAME = "Teams";

        public const string CONTRIBUTORS_TABLE_NAME = "Contributors";

        public const string CONTRIBUTOR_TEAMS_TABLE_NAME = "ContributorTeams";

        public const string CONTRIBUTOR_TO_TEAM_FK = "ContributorTeamId";

        public const string TEAM_TO_PLAYER_FK = "CaptainId";
    }
}