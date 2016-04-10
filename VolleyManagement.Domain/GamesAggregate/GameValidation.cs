namespace VolleyManagement.Domain.GamesAggregate
{
    /// <summary>
    /// Game validation class.
    /// </summary>
    public static class GameValidation
    {
        public const int FREE_DAY_TEAM_ID = 0; 

        /// <summary>
        /// Determines whether the home team and the away team are the same.
        /// </summary>
        /// <param name="homeTeamId">Identifier of the home team.</param>
        /// <param name="awayTeamId">Identifier of the away team.</param>
        /// <returns>True team are the same; otherwise, false.</returns>
        public static bool AreTheSameTeams(int homeTeamId, int awayTeamId)
        {
            return homeTeamId == awayTeamId;
        }

        /// <summary>
        /// Determines whether the home and away teams are same acordingly 
        /// </summary>
        /// <param name="firstGame">First game in tournament</param>
        /// <param name="secondGame">Second game in tournament</param>
        /// <returns>True if gamse contain same home and away teams accordingly</returns>
        public static bool AreTheSameTeamsInGames(GameResultDto firstGame, GameResultDto secondGame)
        {
            return AreTheSameTeams(firstGame.AwayTeamId, secondGame.AwayTeamId)
                && AreTheSameTeams(firstGame.HomeTeamId, secondGame.HomeTeamId);
        }

        /// <summary>
        /// Checks if one team from new game participates in original game 
        /// </summary>
        /// <param name="originalGame">Game where teams are compared to new game's teams</param>
        /// <param name="newGame">Game which tems are compared to original game's teams</param>
        /// <returns>True if one of new game's teams participates in original game</returns>
        public static bool IsTheSameTeamInTwoGames(GameResultDto originalGame, GameResultDto newGame)
        {
            return originalGame.HomeTeamId == newGame.HomeTeamId
                || originalGame.HomeTeamId == newGame.AwayTeamId
                || originalGame.AwayTeamId == newGame.HomeTeamId
                || originalGame.AwayTeamId == newGame.AwayTeamId; 
        }

        public static bool IsFreeDayGame(GameResultDto game)
        {
            return game.HomeTeamId == FREE_DAY_TEAM_ID; 
        }
    }
}