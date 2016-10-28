namespace VolleyManagement.Data.Queries.GameResult
{
    using System.Collections.Generic;
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Represents criteria to get games from specified round numbers in tournament
    /// </summary>
    public class GamesByRoundCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets orr sets id of the tournament
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the collection of round numbers
        /// </summary>
        public IList<byte> RoundNumbers { get; set; }
    }
}
