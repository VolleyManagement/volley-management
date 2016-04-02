namespace VolleyManagement.Domain.GamesAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Game validation class.
    /// </summary>
    class GameValidation
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
