namespace VolleyManagement.Domain.GamesAggregate
{
    /// <summary>
    /// Game validation class.
    /// </summary>
    public static class GameValidation
    {
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
    }
}