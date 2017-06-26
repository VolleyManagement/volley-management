namespace VolleyManagement.Data.Queries.GameResult
{
    using System.Collections.Generic;
    using Contracts;

    /// <summary>
    /// Represents criteria to get game by number in tournament
    /// </summary>
    public class GameByNumberCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets id of the tournament
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets game number
        /// </summary>
        public int GameNumber { get; set; }
    }
}
