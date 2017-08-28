namespace VolleyManagement.Data.Queries.Tournament
{
    using System;
    using Contracts;

    /// <summary>
    /// Represents criteria for finding old tournaments.
    /// </summary>
    public class OldTournamentsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets date criteria
        /// </summary>
        public DateTime CheckDate { get; set; }
    }
}
