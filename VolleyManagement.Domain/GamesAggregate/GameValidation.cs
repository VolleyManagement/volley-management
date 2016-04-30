namespace VolleyManagement.Domain.GamesAggregate
{
    /// <summary>
    /// Game validation class.
    /// </summary>
    public static class GameValidation
    {
        public const int MAX_DUPLICATE_GAMES_IN_SCHEMA_TWO = 2;

        /// <summary>
        /// Determines whether the home team and the away team are the same.
        /// </summary>
        /// <param name="firsTeamId">Identifier of the home team.</param>
        /// <param name="secondTeamId">Identifier of the away team.</param>
        /// <returns>True team are the same; otherwise, false.</returns>
        public static bool AreTheSameTeams(int? firsTeamId, int? secondTeamId)
        {
            return firsTeamId == secondTeamId;
        }

        /// <summary>
        /// Determines whether the home and away teams are same accordingly
        /// </summary>
        /// <param name="firstGame">First game in tournament</param>
        /// <param name="secondGame">Second game in tournament</param>
        /// <returns>True if games contain same home and away teams accordingly</returns>
        public static bool AreSameOrderTeamsInGames(GameResultDto firstGame, Game secondGame)
        {
            return AreTheSameTeams(firstGame.AwayTeamId, secondGame.AwayTeamId)
                && AreTheSameTeams(firstGame.HomeTeamId, secondGame.HomeTeamId);
        }

        /// <summary>
        /// Verifies if teams are same in two different games ignoring order
        /// </summary>
        /// <param name="firstGame">First game to check</param>
        /// <param name="secondGame">Second game to check</param>
        /// <returns>true if teams are the same in games</returns>
        public static bool AreSameTeamsInGames(GameResultDto firstGame, Game secondGame)
        {
            return (AreTheSameTeams(firstGame.AwayTeamId, secondGame.AwayTeamId)
                && AreTheSameTeams(firstGame.HomeTeamId, secondGame.HomeTeamId))
                || (AreTheSameTeams(firstGame.AwayTeamId, secondGame.HomeTeamId)
                && AreTheSameTeams(firstGame.HomeTeamId, secondGame.AwayTeamId));
        }

        /// <summary>
        /// Checks if one team from new game participates in original game
        /// </summary>
        /// <param name="originalGame">Game which teams are compared to new game's teams</param>
        /// <param name="newGame">Game which teams are compared to original game's teams</param>
        /// <returns>True if one of new game's teams participates in original game</returns>
        public static bool IsTheSameTeamInTwoGames(GameResultDto originalGame, Game newGame)
        {
            return originalGame.HomeTeamId == newGame.HomeTeamId
                || originalGame.HomeTeamId == newGame.AwayTeamId
                || originalGame.AwayTeamId == newGame.HomeTeamId
                || originalGame.AwayTeamId == newGame.AwayTeamId;
        }

        /// <summary>
        /// Checks if team is scheduled to a free day
        /// </summary>
        /// <param name="teamId">Id of the team</param>
        /// <returns>True if game is scheduled free day</returns>
        public static bool IsFreeDayTeam(int? teamId)
        {
            return teamId == null;
        }

        /// <summary>
        /// Checks if team in the game is scheduled to free day
        /// </summary>
        /// <param name="game">Game to check</param>
        /// <returns>True if team in game is scheduled in free day</returns>
        public static bool IsFreeDayGame(Game game)
        {
            return !game.AwayTeamId.HasValue;
        }

        /// <summary>
        /// Checks if team in the game is scheduled to free day
        /// </summary>
        /// <param name="gameResult">Game result to check</param>
        /// <returns>True if team in game is scheduled in free day</returns>
        public static bool IsFreeDayGame(GameResultDto gameResult)
        {
            return !gameResult.AwayTeamId.HasValue;
        }
    }
}